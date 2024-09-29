using Fluxor;
using Microsoft.AspNetCore.Components;
using RecipeManager.Shared.Models;
using RecipeManager.Web.Store.RecipeStore;

namespace RecipeManager.Web.Pages;

public partial class ViewRecipePage
{
    [Parameter]
    public string? RecipeId { get; set; }

    [Inject]
    private IState<RecipeState> RecipeState { get; set; } = default!;

    private Recipe Recipe => RecipeState.Value.RecipieCollections
                                            .SelectMany(c => c.Recipes)
                                            .FirstOrDefault(r => r.RecipeId == Guid.Parse(RecipeId!))
                                            ?? new Recipe();

    protected override void OnParametersSet()
    {
        RecipeId = RecipeId ?? $"{Guid.Empty}";
        base.OnParametersSet();
    }
}
