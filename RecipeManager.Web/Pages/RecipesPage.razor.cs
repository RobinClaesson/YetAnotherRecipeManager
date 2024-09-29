using Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RecipeManager.Web.Models.RecipeView;
using RecipeManager.Web.Store.RecipeStore;

namespace RecipeManager.Web.Pages;

public partial class RecipesPage
{
    [Inject]
    IState<RecipeState> RecipeState { get; set; } = default!;

    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;

    private IEnumerable<RecipeListing> RecipeListings 
        => RecipeState.Value.RecipieCollections.
                SelectMany(c =>
                    c.Recipes.Select(r => new RecipeListing(r, c.Source?.Name ?? string.Empty))
                );

    private static string ReadableTags(RecipeListing recipe)
        => string.Join(", ", recipe.Tags.OrderBy(t => t));

    private void ViewRecipe(Guid recipeId)
        => NavigationManager.NavigateTo($"/recipes/{recipeId}");
}
