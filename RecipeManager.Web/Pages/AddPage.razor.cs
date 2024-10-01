using Fluxor;
using Microsoft.AspNetCore.Components;
using RecipeManager.Shared.Contracts;
using RecipeManager.Web.Store.RecipeStore;

namespace RecipeManager.Web.Pages;

public partial class AddPage
{
    [Inject]
    IState<RecipeState> RecipeState { get; set; } = default!;

}
