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
        {
            var local = new RecipeSource
            {
                Name = Constants.LocalRecipeSourceName,
                Url = Constants.LocalRecipeSourceUrl
            };

            var localJson = JsonSerializer.Serialize(local);

            await _localStorage.SetItemAsStringAsync(Constants.LocalStorageRecipeSources, $"[{localJson}]");
        }

        if (!(await _localStorage.ContainKeyAsync(Constants.LocalStorageLocalRecipes)))
        {
            await _localStorage.SetItemAsStringAsync(Constants.LocalStorageLocalRecipes, "[]");
        }
    }

    [EffectMethod]
    public async Task OnAppLoadedAction(AppLoadedAction action, IDispatcher dispatcher)
    {
        await InitLocalStorage();

        var recipeSources = await _localStorage.GetItemAsync<List<RecipeSource>>(Constants.LocalStorageRecipeSources);

        if (recipeSources is null)
            return;

        foreach (var source in recipeSources)
        {
            if (source.Url == Constants.LocalRecipeSourceUrl)
            {
                var localRecipes = await _localStorage.GetItemAsync<List<Recipe>>(Constants.LocalStorageLocalRecipes);
                if (localRecipes is not null)
                    dispatcher.Dispatch(new RecipesFetchedFromSourceAction(source, localRecipes));
            }
            else
            {
                var recipes = await _httpClient.GetFromJsonAsync<List<Recipe>>($"{source.Url}/api/Recipe/GetRecipesFull");
                if (recipes is not null)
                    dispatcher.Dispatch(new RecipesFetchedFromSourceAction(source, recipes));
            }
        }
    }
}
