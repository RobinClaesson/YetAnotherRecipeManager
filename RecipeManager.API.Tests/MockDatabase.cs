using Microsoft.EntityFrameworkCore;
using RecipeManager.Shared.Db;
using RecipeManager.Shared.Models;

namespace RecipeManager.API.Tests;

internal static class MockDatabase
{
	public static RecipeContext DatabaseSetup()
	{
		var options = new DbContextOptionsBuilder<RecipeContext>();
		options.UseSqlite("Data Source=MockDatabase.db");

		var context = new RecipeContext(options.Options);
		context.Database.EnsureCreated();

		context.Recipes.AddRange(MockRecipes);
		context.Ingredients.AddRange(MockIngredients);
		context.Instructions.AddRange(MockInstructions);
		context.SaveChanges();

		return new RecipeContext(options.Options);
    }

	public static void DatabaseTeardown(RecipeContext context)
	{
		context.Database.EnsureDeleted();
	}

	public static readonly List<Recipe> MockRecipes = new() { 
		new()
		{
			RecipeId = Guid.NewGuid(),
			Name = "Milk and Cereal",
			Description = "A simple breakfast",
			Tags = new() { "breakfast", "simple" },
			Servings = 1
		},
		new()
		{
            RecipeId = Guid.NewGuid(),
            Name = "Peanut Butter and Jelly Sandwich",
            Description = "A simple lunch",
            Tags = new() { "lunch", "simple" },
            Servings = 1
        }
	};

	public static readonly List<Ingredient> MockIngredients = new()
	{
		new()
		{
			IngredientId = Guid.NewGuid(),
			Name = "Milk",
			Quantity = 2,
			Unit = Units.Cup,
			RecipeId = MockRecipes[0].RecipeId
		},
		new()
		{
            IngredientId = Guid.NewGuid(),
            Name = "Cereal",
            Quantity = 150,
            Unit = Units.Grams,
            RecipeId = MockRecipes[0].RecipeId
        },
		new()
		{
            IngredientId = Guid.NewGuid(),
            Name = "Peanut Butter",
            Quantity = 2,
            Unit = Units.Tablespoon,
            RecipeId = MockRecipes[1].RecipeId
        },
		new()
		{
            IngredientId = Guid.NewGuid(),
            Name = "Jelly",
            Quantity = 1,
            Unit = Units.Tablespoon,
            RecipeId = MockRecipes[1].RecipeId
        },
		new()
		{
			IngredientId = Guid.NewGuid(),
			Name = "Breadslices",
			Quantity = 2,
			Unit = Units.Piece,
			RecipeId = MockRecipes[1].RecipeId
		},
		new()
		{
			IngredientId = Guid.NewGuid(),
			Name = "Milk",
			Quantity = 1,
			Unit = Units.Cup,
			RecipeId = MockRecipes[1].RecipeId
		}
	};

	public static List<Instruction> MockInstructions = new()
	{
		new()
		{
			InstructionId = Guid.NewGuid(),
			Name = "Cereal",
			Order = 1,
			Description = "Pour cereal into bowl",
			RecipeId = MockRecipes[0].RecipeId
		},
		new()
		{
            InstructionId = Guid.NewGuid(),
            Name = "Milk",
            Order = 2,
            Description = "Pour milk over the cereal",
            RecipeId = MockRecipes[0].RecipeId
        },
		new()
		{
            InstructionId = Guid.NewGuid(),
            Name = "Peanut Butter",
            Order = 1,
            Description = "Spread peanut butter on one slice of bread",
            RecipeId = MockRecipes[1].RecipeId
        },
		new()
		{
            InstructionId = Guid.NewGuid(),
            Name = "Jelly",
            Order = 2,
            Description = "Spread jelly on the other slice of bread",
            RecipeId = MockRecipes[1].RecipeId
        },
		new()
		{
            InstructionId = Guid.NewGuid(),
            Name = "Combine",
            Order = 3,
            Description = "Combine the two slices of bread",
            RecipeId = MockRecipes[1].RecipeId
        },
		new()
		{
            InstructionId = Guid.NewGuid(),
            Name = "Milk",
            Order = 4,
            Description = "Pour a glass of milk to enjoy with the sandwitch",
            RecipeId = MockRecipes[1].RecipeId
        }
	};
}
