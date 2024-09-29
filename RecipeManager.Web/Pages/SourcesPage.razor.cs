using Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RecipeManager.Web.Components.Sources;
using RecipeManager.Web.Models.RecipeStore;
using RecipeManager.Web.Store.RecipeStore;

namespace RecipeManager.Web.Pages;

public partial class SourcesPage
{
    [Inject]
    IState<RecipeState> RecipeState { get; set; } = default!;

    [Inject]
    IDialogService DialogService { get; set; } = default!;

    private IEnumerable<RecipeSource> RecipeSources
        => RecipeState.Value.RecipieCollections.Select(r => r.Source);

    private Task AddSourceAsync()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
        };

        return DialogService.ShowAsync<AddSourceDialog>("Add Source", options);
    }
}
