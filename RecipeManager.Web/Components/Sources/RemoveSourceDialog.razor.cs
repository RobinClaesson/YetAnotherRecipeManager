using Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RecipeManager.Web.Models.RecipeStore;
using RecipeManager.Web.Store.RecipeStore;

namespace RecipeManager.Web.Components.Sources;

public partial class RemoveSourceDialog
{
    [Inject]
    IDispatcher Dispatcher { get; set; } = default!;

    [Parameter]
    public string Name { get; set; } = string.Empty;

    [Parameter]
    public string Url { get; set; } = string.Empty;

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }

    private void Cancel() => MudDialog.Cancel();

    private void Remove()
    {
        Dispatcher.Dispatch(new SourceRemovedAction(new RecipeSource
        {
            Name = Name,
            Url = Url
        }));
        MudDialog.Close(DialogResult.Ok(true));
    }
}
