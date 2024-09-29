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

    public static UpdateRecipeContract FromModel(Recipe recipe)
        => new UpdateRecipeContract
        {
            RecipeId = recipe.RecipeId,
            Name = recipe.Name,
            Description = recipe.Description,
            Tags = recipe.Tags,
            Servings = recipe.Servings,
            Ingredients = recipe.Ingredients.Select(UpdateIngredientContract.FromModel).ToList(),
            Instructions = recipe.Instructions.Select(UpdateInstructionContract.FromModel).ToList()
        };

    public Recipe ToModel()
        => new Recipe
        {
            RecipeId = RecipeId,
            Name = Name ?? string.Empty,
            Description = Description ?? string.Empty,
            Tags = Tags ?? new List<string>(),
            Servings = Servings ?? 0,
            Ingredients = Ingredients?.Select(i => i.ToModel(RecipeId)).ToList() ?? new List<Ingredient>(),
            Instructions = Instructions?.Select(i => i.ToModel(RecipeId)).ToList() ?? new List<Instruction>()
        };
}
