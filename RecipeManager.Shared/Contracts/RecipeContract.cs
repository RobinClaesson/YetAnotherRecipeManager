using Microsoft.VisualBasic;
using RecipeManager.Shared.Models;
using System.Text;

namespace RecipeManager.Shared.Contracts;

public record RecipeContract
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();

    public int Servings { get; set; } = 0;

    public List<IngredientContract> Ingredients { get; set; } = new();
    public List<InstructionContract> Instructions { get; set; } = new();

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Name: {Name}");
        sb.AppendLine($"Description: {Description}");
        sb.AppendLine($"Tags: {string.Join(", ", Tags)}");
        sb.AppendLine($"Servings: {Servings}");

        sb.AppendLine("Ingredients: ");
        foreach (var ingredient in Ingredients)
            sb.AppendLine($"\t{ingredient.ToString()}");

        sb.AppendLine("Instructions: ");
        foreach (var instruction in Instructions)
            sb.AppendLine($"\t{instruction.ToString()}");

        return sb.ToString();
    }

    public Recipe ToModel()
    {
        return new Recipe
        {
            Name = Name,
            Description = Description,
            Tags = Tags,
            Servings = Servings,
            Ingredients = Ingredients.Select(i => i.ToModel()).ToList(),
            Instructions = Instructions.Select(i => i.ToModel()).ToList()
        };
    }

    public static RecipeContract FromModel(Recipe recipeModel)
    {
        return new RecipeContract
        {
            Name = recipeModel.Name,
            Description = recipeModel.Description,
            Tags = recipeModel.Tags,
            Servings = recipeModel.Servings,
            Ingredients = recipeModel.Ingredients.Select(i => IngredientContract.FromModel(i)).ToList(),
            Instructions = recipeModel.Instructions.Select(i => InstructionContract.FromModel(i)).ToList()
        };
    }
}
