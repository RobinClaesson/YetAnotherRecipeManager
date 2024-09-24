using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipeManager.Shared.Models;

public record Recipe
{
    [Key]
    public Guid RecipeId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();

    public int Servings { get; set; } = 0;

    public List<Ingredient> Ingredients { get; set; } = new();
    public List<Instruction> Instructions { get; set; } = new();

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

    /// <summary>
    /// Fill in missing references to the parent recipe in the child ingredients and instructions.
    /// </summary>
    public void FillMissingParentReferencesInChildren()
    {
        foreach (var ingredient in Ingredients)
            if (ingredient.Recipe == null)
                ingredient.Recipe = this;

        foreach (var instruction in Instructions)
            if (instruction.Recipe == null)
                instruction.Recipe = this;
    }
}
