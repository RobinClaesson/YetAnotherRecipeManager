using Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RecipeManager.Web.Store.ViewStore;
using System.Runtime.InteropServices;

namespace RecipeManager.Web.Components.MainLayout;

public partial class Navbar
{
    [Inject]
    private IState<ViewState> ViewState { get; set; } = default!;

    [Inject]
    private IDispatcher Dispatcher { get; set; } = default!;

    private void ToggleDarkMode()
    {
        Dispatcher.Dispatch(new DarkModeSetAction(!ViewState.Value.DarkMode));
    }

    private string DarkModeIcon => ViewState.Value.DarkMode ? @Icons.Material.Outlined.LightMode : @Icons.Material.Outlined.DarkMode;
}
