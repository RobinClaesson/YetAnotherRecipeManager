using RecipeManager.Shared.Models;

namespace RecipeManager.Shared.Contracts;

public record UpdateIngredientContract
{
    public Guid? IngredientId { get; set; }
    public string? Name { get; set; }
    public double? Quantity { get; set; }
    public Units? Unit { get; set; }
}
