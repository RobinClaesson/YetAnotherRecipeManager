using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using RecipeManager.Web.Store.CommonStore;

namespace RecipeManager.Web.Components.MainLayout;

public partial class ToastProvider
{
    [Inject]
    IToastService ToastService { get; set; } = default!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        SubscribeToAction<ErrorOccurredAction>(ShowErrorToast);
    }

    public void ShowErrorToast(ErrorOccurredAction action) => ToastService.ShowError(action.ErrorMessage);
}
