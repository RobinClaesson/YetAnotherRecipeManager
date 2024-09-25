using Blazored.LocalStorage;
using Fluxor;
using RecipeManager.Web.Store.CommonStore;

namespace RecipeManager.Web.Store.ViewStore;

public class ViewEffects
{
    private readonly ILocalStorageService _localStorage;

    public ViewEffects(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    [EffectMethod]
    public async Task OnAppLoadedAction(AppLoadedAction _, IDispatcher dispatcher)
    {
        if (!(await _localStorage.ContainKeyAsync(Constants.LocalStorageDarkMode)))
        {
            await _localStorage.SetItemAsync(Constants.LocalStorageDarkMode, false);
            return;  
        }

        var darkMode = await _localStorage.GetItemAsync<bool>(Constants.LocalStorageDarkMode);
        dispatcher.Dispatch(new DarkModeLoadedAction(darkMode));
    }

    [EffectMethod]
    public async Task OnDarkModeSetAction(DarkModeSetAction action, IDispatcher dispatcher)
    {   
        await _localStorage.SetItemAsync(Constants.LocalStorageDarkMode, action.DarkMode);
    }
}
