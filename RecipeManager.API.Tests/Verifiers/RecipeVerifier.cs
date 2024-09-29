using FluentAssertions;
using RecipeManager.Shared.Models;

namespace RecipeManager.API.Tests.Verifiers;

internal static class RecipeVerifier
{
    public static void VerifyRecipeIEnumerable(IEnumerable<Recipe> result, IEnumerable<Recipe> expected)
    {
        result.Should().HaveCount(expected.Count());

        var orderedResult = result.OrderBy(r => r.RecipeId);
        var orderedExpected = expected.OrderBy(r => r.RecipeId);

        for (int i = 0; i < result.Count(); i++)
        {
            var expectedRecipe = orderedExpected.ElementAt(i);
            var resultRecipe = orderedResult.ElementAt(i);

            VerifyRecipe(resultRecipe, expectedRecipe);
        }
    }

    //Manual walkthrough of model comparison as the cyclical structure of the models    
    //throws off the FluentAssertions comparison. (Cound not get ExcludingNestedObjects to work)
    public static void VerifyRecipe(Recipe result, Recipe expected)
    {
        result.Should().NotBeNull();
        result.RecipeId.Should().Be(expected.RecipeId);
        result.Name.Should().BeEquivalentTo(expected.Name);
        result.Description.Should().BeEquivalentTo(expected.Description);
        result.Tags.Should().BeEquivalentTo(expected.Tags);
        result.Servings.Should().Be(expected.Servings);

        var expectedIngredients = expected.Ingredients.OrderBy(i => i.IngredientId);
        var resultIngredients = result.Ingredients.OrderBy(i => i.IngredientId);

        resultIngredients.Should().NotBeNullOrEmpty();
        for (int j = 0; j < resultIngredients.Count(); j++)
        {
            var expectedIngredient = expectedIngredients.ElementAt(j);
            var resultIngredient = resultIngredients.ElementAt(j);

            resultIngredient.Should().NotBeNull();
            resultIngredient.IngredientId.Should().Be(expectedIngredient.IngredientId);
            resultIngredient.Name.Should().BeEquivalentTo(expectedIngredient.Name);
            resultIngredient.Quantity.Should().Be(expectedIngredient.Quantity);
            resultIngredient.Unit.Should().Be(expectedIngredient.Unit);
            resultIngredient.RecipeId.Should().Be(expectedIngredient.RecipeId);
        }

        var expectedInstructions = expected.Instructions.OrderBy(i => i.InstructionId);
        var resultInstructions = result.Instructions.OrderBy(i => i.InstructionId);

        resultInstructions.Should().NotBeNullOrEmpty();
        for (int j = 0; j < resultInstructions.Count(); j++)
        {
            var expectedInstruction = expectedInstructions.ElementAt(j);
            var resultInstruction = resultInstructions.ElementAt(j);

            resultInstruction.Should().NotBeNull();
            resultInstruction.InstructionId.Should().Be(expectedInstruction.InstructionId);
            resultInstruction.Name.Should().BeEquivalentTo(expectedInstruction.Name);
            resultInstruction.Order.Should().Be(expectedInstruction.Order);
            resultInstruction.Description.Should().BeEquivalentTo(expectedInstruction.Description);
            resultInstruction.RecipeId.Should().Be(expectedInstruction.RecipeId);
        }
    }

    public static void VerifyRecipeOnlyEmptyCheckChildrenId(Recipe result, Recipe expected)
    {
        result.Should().NotBeNull();
        result.RecipeId.Should().Be(expected.RecipeId);
        result.Name.Should().BeEquivalentTo(expected.Name);
        result.Description.Should().BeEquivalentTo(expected.Description);
        result.Tags.Should().BeEquivalentTo(expected.Tags);
        result.Servings.Should().Be(expected.Servings);

        var expectedIngredients = expected.Ingredients.OrderBy(i => i.Name);
        var resultIngredients = result.Ingredients.OrderBy(i => i.Name);

        resultIngredients.Should().NotBeNullOrEmpty();
        for (int j = 0; j < resultIngredients.Count(); j++)
        {
            var expectedIngredient = expectedIngredients.ElementAt(j);
            var resultIngredient = resultIngredients.ElementAt(j);

            resultIngredient.Should().NotBeNull();
            resultIngredient.IngredientId.Should().NotBeEmpty();
            resultIngredient.Name.Should().BeEquivalentTo(expectedIngredient.Name);
            resultIngredient.Quantity.Should().Be(expectedIngredient.Quantity);
            resultIngredient.Unit.Should().Be(expectedIngredient.Unit);
            resultIngredient.RecipeId.Should().Be(expectedIngredient.RecipeId);
        }

        var expectedInstructions = expected.Instructions.OrderBy(i => i.Order);
        var resultInstructions = result.Instructions.OrderBy(i => i.Order);

        resultInstructions.Should().NotBeNullOrEmpty();
        for (int j = 0; j < resultInstructions.Count(); j++)
        {
            var expectedInstruction = expectedInstructions.ElementAt(j);
            var resultInstruction = resultInstructions.ElementAt(j);

            resultInstruction.Should().NotBeNull();
            resultInstruction.InstructionId.Should().NotBeEmpty();
            resultInstruction.Name.Should().BeEquivalentTo(expectedInstruction.Name);
            resultInstruction.Order.Should().Be(expectedInstruction.Order);
            resultInstruction.Description.Should().BeEquivalentTo(expectedInstruction.Description);
            resultInstruction.RecipeId.Should().Be(expectedInstruction.RecipeId);
        }
    }
}
