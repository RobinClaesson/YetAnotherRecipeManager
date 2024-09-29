using Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RecipeManager.Web.Models.RecipeStore;
using RecipeManager.Web.Store.RecipeStore;
using System.Xml.Linq;

namespace RecipeManager.Web.Components.Sources;

public partial class EditSourceDialog
{
    [Inject]
    IState<RecipeState> RecipeState { get; set; } = default!;

    [Inject]
    IDispatcher Dispatcher { get; set; } = default!;

    [Parameter]
    public string Name { get; set; } = string.Empty;

    [Parameter]
    public string Url { get; set; } = string.Empty;

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }

    private RecipeSource _originalSource = new();

    protected override void OnInitialized()
    {
        _originalSource = new RecipeSource
        {
            Name = $"{Name}",
            Url = $"{Url}"
        };
    }

    private bool NameIsEmpty => string.IsNullOrWhiteSpace(Name);
    private bool NameExists => Name != _originalSource.Name && RecipeState.Value.RecipieCollections.Any(r => r.Source.Name == Name);
    private bool UrlIsEmpty => string.IsNullOrWhiteSpace(Url);
    private bool UrlExists => Url != _originalSource.Url && RecipeState.Value.RecipieCollections.Any(r => r.Source.Url == Url);
    private bool IsSame => Name == _originalSource.Name && Url == _originalSource.Url;
    private bool DisableAply => NameIsEmpty || NameExists || UrlIsEmpty || UrlExists || IsSame;

    private void Cancel() => MudDialog.Cancel();

    private void ApplyEdit()
    {
        var chaged = new RecipeSource
        {
            Name = Name,
            Url = Url
        };
        Dispatcher.Dispatch(new SourceChangedAction(_originalSource, chaged));
        MudDialog.Close(DialogResult.Ok(chaged));
    }
}
