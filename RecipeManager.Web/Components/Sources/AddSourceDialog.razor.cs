using Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RecipeManager.Web.Models.RecipeStore;
using RecipeManager.Web.Store.RecipeStore;

namespace RecipeManager.Web.Components.Sources;

public partial class AddSourceDialog
{
    private string _name = string.Empty;
    private string _url = string.Empty;

    [Inject]
    IDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    IState<RecipeState> RecipeState { get; set; } = default!;

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }

    private void Cancel() => MudDialog.Cancel();

    private bool NameIsEmpty => string.IsNullOrWhiteSpace(_name);
    private bool NameExists => RecipeState.Value.RecipieCollections.Any(r => r.Source.Name == _name);
    private bool UrlIsEmpty => string.IsNullOrWhiteSpace(_url);
    private bool UrlExists => RecipeState.Value.RecipieCollections.Any(r => r.Source.Url == _url);

    private bool DisableAdd => NameIsEmpty || NameExists || UrlIsEmpty || UrlExists;

    private void Add()
    {
        var source = new RecipeSource
        {
            Name = _name,
            Url = _url
        };
        Dispatcher.Dispatch(new SourceAddedAction(source));
        MudDialog.Close(DialogResult.Ok(source));
    }
}
