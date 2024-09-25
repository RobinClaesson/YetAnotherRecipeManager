using RecipeManager.Shared.Models;

namespace RecipeManager.Web.Models.RecipeView;

public record RecipeListing : Recipe
{
    public string SourceName { get; set;} = string.Empty;

    public RecipeListing(Recipe recipe, string sourceName)
    {
        RecipeId = recipe.RecipeId;
        Name = recipe.Name;
        Description = recipe.Description;
        Tags = recipe.Tags;
        Servings = recipe.Servings;
        Ingredients = recipe.Ingredients;
        Instructions = recipe.Instructions;
        SourceName = sourceName;
    }
}
