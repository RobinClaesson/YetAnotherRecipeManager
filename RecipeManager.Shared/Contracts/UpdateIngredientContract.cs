﻿using RecipeManager.Shared.Models;

namespace RecipeManager.Shared.Contracts;

public record UpdateIngredientContract
{
    public Guid? IngredientId { get; set; }
    public string? Name { get; set; }
    public double? Quantity { get; set; }
    public Units? Unit { get; set; }

    public static UpdateIngredientContract FromModel(Ingredient ingredient)
        => new UpdateIngredientContract
        {
            IngredientId = ingredient.IngredientId,
            Name = ingredient.Name,
            Quantity = ingredient.Quantity,
            Unit = ingredient.Unit
        };

    public Ingredient ToModel(Guid recipeId)
        => new Ingredient
        {
            IngredientId = IngredientId ?? default,
            Name = Name ?? string.Empty,
            Quantity = Quantity ?? 0,
            Unit = Unit ?? Units.None,
            RecipeId = recipeId
        };
}
