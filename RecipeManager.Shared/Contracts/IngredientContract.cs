using RecipeManager.Shared.Models;

namespace RecipeManager.Shared.Contracts;

public record IngredientContract
{
    public string Name { get; set; } = string.Empty;
    public double Quantity { get; set; } = 0;
    public Units Unit { get; set; } = Units.None;
    
    public override string ToString()
    {
        return $"{Name} {Quantity} {Unit}";
    }

    public Ingredient ToModel()
    {
        return new Ingredient
        {
            Name = Name,
            Quantity = Quantity,
            Unit = Unit
        };
    }

    public static IngredientContract FromModel(Ingredient ingredient)
    {
        return new IngredientContract
        {
            Name = ingredient.Name,
            Quantity = ingredient.Quantity,
            Unit = ingredient.Unit
        };
    }
}
