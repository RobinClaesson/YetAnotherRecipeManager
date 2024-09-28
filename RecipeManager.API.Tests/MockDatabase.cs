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
			RecipeId = new("d42f78fd-84b4-4a0f-a6f5-28576ea386d2"),
			Name = "Milk and Cereal",
			Description = "A simple breakfast",
			Tags = new() { "breakfast", "simple" },
			Servings = 1
		},
		new()
		{
            RecipeId = new("df5c4f86-ca8c-4077-a397-76aa1edc2a91"),
            Name = "Peanut Butter and Jelly Sandwich",
            Description = "A simple lunch",
            Tags = new() { "lunch", "simple" },
            Servings = 1
        }
	};
}
