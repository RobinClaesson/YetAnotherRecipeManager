using System.ComponentModel.DataAnnotations;

namespace RecipeManager.Shared.Models;

public record Ingredient
{
    [Key]
    public Guid IngredientId { get; set; }

    public string Name { get; set; } = string.Empty;
    public double Quantity { get; set; } = 0;
    public Units Unit { get; set; } = Units.None;

    //Parent Recipe
    public Guid RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;
}
