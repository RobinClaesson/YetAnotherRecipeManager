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

    [Inject]
    IDispatcher Dispatcher { get; set; } = default!;

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

    private Task RemoveSourceAsync(RecipeSource source)
    {
        var parameters = new DialogParameters
        {
            { nameof(RemoveSourceDialog.Name), source.Name },
            { nameof(RemoveSourceDialog.Url), source.Url }
        };

        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
        };

        return DialogService.ShowAsync<RemoveSourceDialog>("Remove Source", parameters, options);
    }

    private Task EditSourceAsync(RecipeSource source)
    {
        var parameters = new DialogParameters
        {
            { nameof(EditSourceDialog.Name), source.Name },
            { nameof(EditSourceDialog.Url), source.Url }
        };

        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
        };

        return DialogService.ShowAsync<EditSourceDialog>("Edit Source", parameters, options);
    }

    private Task ResetLocalSourceRecipesAsync()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
        };

        return DialogService.ShowAsync<ResetLocalDialog>("Reset Local Source", options);
    }

    private void ReloadSource(RecipeSource source)
    {
        Dispatcher.Dispatch(new ReloadSourceAction(source));
    }
}
