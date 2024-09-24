using RecipeManager.Shared.Models;

namespace RecipeManager.Web.Store.RecipeStore.Models;

public record RecipieCollection
{
    public RecipeSource? Source { get; init; } = null;
    public List<Recipe> Recipes { get; init; } = new List<Recipe>();
}
