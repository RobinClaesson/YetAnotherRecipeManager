using Blazored.LocalStorage;
using Fluxor;
using RecipeManager.Shared.Models;
using RecipeManager.Web.Models.RecipeStore;
using RecipeManager.Web.Store.CommonStore;
using System.Net.Http.Json;
using System.Text.Json;

namespace RecipeManager.Web.Store.RecipeStore;

public class RecipeEffects
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;

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

    [EffectMethod]
    public async Task OnAppLoadedAction(AppLoadedAction action, IDispatcher dispatcher)
    {
        await InitLocalStorage();

        //Local Storage recipes
        var localStorageSource = new RecipeSource
        {
            Name = Constants.LocalRecipeSourceName,
            Url = Constants.LocalRecipeSourceUrl
        };
        var localRecipes = await _localStorage.GetItemAsync<List<Recipe>>(Constants.LocalStorageLocalRecipes);
        if (localRecipes is not null)
            dispatcher.Dispatch(new RecipesLoadedFromLocalStorageAction(localRecipes));

        //Other sources
        var recipeSources = await _localStorage.GetItemAsync<List<RecipeSource>>(Constants.LocalStorageRecipeSources);

        foreach (var source in recipeSources!)
        {
            dispatcher.Dispatch(new SourceLoadedFromLocalStorageAction(source));
            var recipes = await _httpClient.GetFromJsonAsync<List<Recipe>>($"{source.Url}/api/Recipe/GetRecipesFull");
            if (recipes is not null)
                dispatcher.Dispatch(new RecipesFetchedFromSourceAction(source, recipes));
        }
    }

    [EffectMethod]
    public async Task OnSourceAddedAction(SourceAddedAction action, IDispatcher dispatcher)
    {
        await InitLocalStorage();

        var recipeSources = await _localStorage.GetItemAsync<List<RecipeSource>>(Constants.LocalStorageRecipeSources);
        recipeSources!.Add(action.RecipeSource);

        var localStorageTask = _localStorage.SetItemAsStringAsync(Constants.LocalStorageRecipeSources, JsonSerializer.Serialize(recipeSources));

        var recipes = await _httpClient.GetFromJsonAsync<List<Recipe>>($"{action.RecipeSource.Url}/api/Recipe/GetRecipesFull");
        if (recipes is not null)
            dispatcher.Dispatch(new RecipesFetchedFromSourceAction(action.RecipeSource, recipes));

        await localStorageTask;
    }
}
