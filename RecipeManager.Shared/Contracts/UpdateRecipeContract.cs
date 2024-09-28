using RecipeManager.Shared.Models;

namespace RecipeManager.Shared.Contracts;

public record UpdateRecipeContract
{
    public Guid RecipeId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<string>? Tags { get; set; }

    public int? Servings { get; set; }

    public List<UpdateIngredientContract>? Ingredients { get; set; }
    public List<UpdateInstructionContract>? Instructions { get; set; }
}
