using RecipeManager.Shared.Models;

namespace RecipeManager.Web.Models.RecipeStore;

public record RecipieCollection
{
    public RecipeSource? Source { get; init; } = null;
    public List<Recipe> Recipes { get; init; } = new List<Recipe>();
}
