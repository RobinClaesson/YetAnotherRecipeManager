using System.ComponentModel.DataAnnotations;

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
}
