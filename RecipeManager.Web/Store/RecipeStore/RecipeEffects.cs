﻿using Blazored.LocalStorage;
using Fluxor;
using RecipeManager.Shared.Models;
using RecipeManager.Web.Models.RecipeStore;
using RecipeManager.Web.Store.CommonStore;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RecipeManager.Web.Store.RecipeStore;

public class RecipeEffects
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _localRecipesSerializerOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    public RecipeEffects(ILocalStorageService localStorage, HttpClient httpClient)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
    }

    private async Task InitLocalStorage()
    {
        if (!(await _localStorage.ContainKeyAsync(Constants.LocalStorageRecipeSources)))
            await _localStorage.SetItemAsStringAsync(Constants.LocalStorageRecipeSources, $"[]");

        if (!(await _localStorage.ContainKeyAsync(Constants.LocalStorageLocalRecipes)))
            await _localStorage.SetItemAsStringAsync(Constants.LocalStorageLocalRecipes, "[]");

    }

    private async Task FetchRecipesFromSource(RecipeSource source, IDispatcher dispatcher)
    {
        try
        {
            var recipes = await _httpClient.GetFromJsonAsync<List<Recipe>>($"{source.Url}/api/Recipe/GetRecipesFull");
            if (recipes is not null)
                dispatcher.Dispatch(new RecipesFetchedFromSourceAction(source, recipes));
        }
        catch
        {
            dispatcher.Dispatch(new ErrorOccurredAction($"Filed to load recipes from '{source.Name}'"));
        }
    }

    [EffectMethod]
    public async Task OnAppLoadedAction(AppLoadedAction action, IDispatcher dispatcher)
    {
        await InitLocalStorage();

        //Local Storage recipes
        try
        {
            var localRecipes = await _localStorage.GetItemAsync<List<Recipe>>(Constants.LocalStorageLocalRecipes);
            if (localRecipes is not null)
                dispatcher.Dispatch(new RecipesLoadedFromLocalStorageAction(localRecipes));
        }
        catch
        {
            dispatcher.Dispatch(new ErrorOccurredAction($"Filed to load local Recipes"));
        }

        //Other sources
        var recipeSources = await _localStorage.GetItemAsync<List<RecipeSource>>(Constants.LocalStorageRecipeSources);
        foreach (var source in recipeSources!)
        {
            dispatcher.Dispatch(new SourceLoadedFromLocalStorageAction(source));
            await FetchRecipesFromSource(source, dispatcher);
        }
    }

    [EffectMethod]
    public async Task OnSourceAddedAction(SourceAddedAction action, IDispatcher dispatcher)
    {
        await InitLocalStorage();

        var recipeSources = await _localStorage.GetItemAsync<List<RecipeSource>>(Constants.LocalStorageRecipeSources);
        recipeSources!.Add(action.RecipeSource);

        var localStorageTask = _localStorage.SetItemAsStringAsync(Constants.LocalStorageRecipeSources, JsonSerializer.Serialize(recipeSources));

        await FetchRecipesFromSource(action.RecipeSource, dispatcher);
        await localStorageTask;
    }

    [EffectMethod]
    public async Task OnSourceRemovedAction(SourceRemovedAction action, IDispatcher dispatcher)
    {
        await InitLocalStorage();

        var recipeSources = await _localStorage.GetItemAsync<List<RecipeSource>>(Constants.LocalStorageRecipeSources);
        recipeSources!.Remove(action.RecipeSource);

        var localStorageTask = _localStorage.SetItemAsStringAsync(Constants.LocalStorageRecipeSources, JsonSerializer.Serialize(recipeSources));

        await localStorageTask;
    }

    [EffectMethod]
    public async Task OnResetLocalSourceRecipesAction(ResetLocalSourceRecipesAction action, IDispatcher dispatcher)
    {
        await _localStorage.SetItemAsStringAsync(Constants.LocalStorageLocalRecipes, "[]");
        dispatcher.Dispatch(new RecipesLoadedFromLocalStorageAction(new List<Recipe>()));
    }

    [EffectMethod]
    public async Task OnSourceChangedAction(SourceChangedAction action, IDispatcher dispatcher)
    {
        await InitLocalStorage();

        var recipeSources = await _localStorage.GetItemAsync<List<RecipeSource>>(Constants.LocalStorageRecipeSources);
        var index = recipeSources!.IndexOf(action.Original);
        recipeSources[index] = action.Updated;

        var localStorageTask = _localStorage.SetItemAsStringAsync(Constants.LocalStorageRecipeSources, JsonSerializer.Serialize(recipeSources));

        if (action.Original.Url != action.Updated.Url)
            await FetchRecipesFromSource(action.Updated, dispatcher);

        await localStorageTask;
    }

    [EffectMethod]
    public async Task OnReloadSourceAction(ReloadSourceAction action, IDispatcher dispatcher)
    {
        await FetchRecipesFromSource(action.RecipeSource, dispatcher);
    }

    [EffectMethod]
    public async Task OnAddRecipeClickedAction(AddRecipeClickedAction action, IDispatcher dispatcher)
    {
        if (action.Source.Name == Constants.LocalRecipeSourceName && action.Source.Url == Constants.LocalRecipeSourceUrl)
        {
            var recipe = action.Recipe.ToModel();
            recipe.RecipeId = Guid.NewGuid();
            foreach (var ingredient in recipe.Ingredients)
            {
                ingredient.RecipeId = recipe.RecipeId;
                ingredient.IngredientId = Guid.NewGuid();
            }
            foreach (var instruction in recipe.Instructions)
            {
                instruction.RecipeId = recipe.RecipeId;
                instruction.InstructionId = Guid.NewGuid();
            }

            await InitLocalStorage();

            var localRecipes = await _localStorage.GetItemAsync<List<Recipe>>(Constants.LocalStorageLocalRecipes) ?? new();
            localRecipes.Add(recipe);

            var serialized = JsonSerializer.Serialize(localRecipes, typeof(List<Recipe>), _localRecipesSerializerOptions);
            await _localStorage.SetItemAsStringAsync(Constants.LocalStorageLocalRecipes, serialized);
            dispatcher.Dispatch(new RecipeAddedAction(recipe, action.Source));
        }
        else
        {
            var httpContent = JsonContent.Create(action.Recipe);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{action.Source.Url}/api/Recipe/AddRecipe", httpContent);
                if (response.IsSuccessStatusCode)
                {
                    var recipeId = await response.Content.ReadFromJsonAsync<Guid>();
                    var recipe = await _httpClient.GetFromJsonAsync<Recipe>($"{action.Source.Url}/api/Recipe/GetRecipe?recipeId={recipeId}");
                    dispatcher.Dispatch(new RecipeAddedAction(recipe!, action.Source));
                }
                else 
                    dispatcher.Dispatch(new AddRecipeToSourceFailedAction());
            }
            catch
            {
                dispatcher.Dispatch(new AddRecipeToSourceFailedAction());
            }
        }

    }
}
