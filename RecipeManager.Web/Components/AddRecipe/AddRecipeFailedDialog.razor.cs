using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace RecipeManager.Web.Components.AddRecipe;

public partial class AddRecipeFailedDialog
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public string SourceName { get; set; } = string.Empty;

    private void Cancel() => MudDialog.Close(DialogResult.Cancel());
    private void ChangeToLocal() => MudDialog.Close(DialogResult.Ok(true));
}
