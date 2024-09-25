using Fluxor;

namespace RecipeManager.Web.Store.ViewStore;

public static class ViewReducers
{
    [ReducerMethod]
    public static ViewState OnDarkModeSetAction(ViewState state, DarkModeSetAction action)
        => state with { DarkMode = action.DarkMode };

    [ReducerMethod]
    public static ViewState OnDarkModeLoadedAction(ViewState state, DarkModeLoadedAction action)
        => state with { DarkMode = action.DarkMode };
}
