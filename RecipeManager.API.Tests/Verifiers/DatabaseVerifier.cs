using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Shared.Db;

namespace RecipeManager.API.Tests.Verifiers;

internal static class DatabaseVerifier
{
    public static void VerifyUnchangedDatabase(RecipeContext context)
    {
        context.Recipes.Count().Should().Be(MockDatabase.MockRecipes.Count);
        context.Ingredients.Count().Should().Be(MockDatabase.MockIngredients.Count);
        context.Instructions.Count().Should().Be(MockDatabase.MockInstructions.Count);

        var resultDatabase = context.Recipes.Include(r => r.Ingredients)
                                                    .Include(r => r.Instructions);

        RecipeVerifier.VerifyRecipeIEnumerable(resultDatabase, MockDatabase.MockRecipes);
    }
}
