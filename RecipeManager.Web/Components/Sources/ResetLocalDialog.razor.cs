using Microsoft.AspNetCore.Components;
using MudBlazor;
using Fluxor;
using RecipeManager.Web.Store.RecipeStore;

namespace RecipeManager.Web.Components.Sources;

public partial class ResetLocalDialog
{
    [Inject]
    IDispatcher Dispatcher { get; set; } = default!;

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }

    private void Cancel() => MudDialog.Cancel();

    private void ResetLocal()
    {
        Dispatcher.Dispatch(new ResetLocalSourceRecipesAction());
        MudDialog.Close(DialogResult.Ok(true));
    }
}
