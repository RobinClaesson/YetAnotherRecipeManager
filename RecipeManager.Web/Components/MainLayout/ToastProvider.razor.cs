using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using RecipeManager.Web.Store.CommonStore;
using RecipeManager.Web.Store.RecipeStore;

namespace RecipeManager.Web.Components.MainLayout;

public partial class ToastProvider
{
    [Inject]
    IToastService ToastService { get; set; } = default!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        SubscribeToAction<ErrorOccurredAction>(ShowErrorToast);

        SubscribeToAction<RecipeAddedAction>(ShowRecipeAddedToast);
    }

    public void ShowErrorToast(ErrorOccurredAction action) => ToastService.ShowError(action.ErrorMessage);

    public void ShowRecipeAddedToast(RecipeAddedAction action) => ToastService.ShowSuccess($"Recipe '{action.Recipe.Name}' added to '{action.Source.Name}'!");
}
