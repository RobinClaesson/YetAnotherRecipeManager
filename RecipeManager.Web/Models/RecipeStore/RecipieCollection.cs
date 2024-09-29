using RecipeManager.Shared.Models;

namespace RecipeManager.Web.Models.RecipeStore;

public record RecipieCollection
{
    public RecipeSource Source { get; init; } = new();
    public List<Recipe> Recipes { get; init; } = new();
}
