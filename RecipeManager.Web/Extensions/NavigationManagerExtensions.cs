using Microsoft.AspNetCore.Components;
using System.Collections.Specialized;

namespace RecipeManager.Web.Extensions.Navbar;

public static class NavigationManagerExtensions
{
    public static string CurrentPage(this NavigationManager navigationManager)
        => navigationManager.ExtensionUri().Split('/').First();

    public static string ExtensionUri(this NavigationManager navigationManager)
        => navigationManager.Uri.Replace(navigationManager.BaseUri, "");
}
