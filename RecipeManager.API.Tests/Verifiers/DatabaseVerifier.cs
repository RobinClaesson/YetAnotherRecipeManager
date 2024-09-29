using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Shared.Db;

namespace RecipeManager.API.Tests.Verifiers;

internal static class DatabaseVerifier
{
    public static void VerifyUnchangedDatabase(MockDatabase database)
    {
        database.RecipeContext.Recipes.Count().Should().Be(database.MockRecipes.Count);
        database.RecipeContext.Ingredients.Count().Should().Be(database.MockIngredients.Count);
        database.RecipeContext.Instructions.Count().Should().Be(database.MockInstructions.Count);

        var resultDatabase = database.RecipeContext.Recipes.Include(r => r.Ingredients)
                                                    .Include(r => r.Instructions);

        RecipeVerifier.VerifyRecipeIEnumerable(resultDatabase, database.MockRecipes);
    }
}
