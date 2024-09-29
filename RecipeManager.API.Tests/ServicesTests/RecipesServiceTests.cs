using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Bson;
using RecipeManager.API.Services;
using RecipeManager.API.Tests.Verifiers;
using RecipeManager.Shared.Contracts;
using RecipeManager.Shared.Db;
using RecipeManager.Shared.Models;

namespace RecipeManager.API.Tests.ServicesTests
{
    public class RecipesServiceTests
    {
        private RecipesService _target;
        private MockDatabase _database;

        [SetUp]
        public void Setup()
        {
            _database = new();
            _target = new(_database.RecipeContext);
        }

        [TearDown]
        public void TearDown()
        {
            _database.Dispose();
        }

        [Test]
        public void GetRecipeIds_ReturnsRecipeIds()
        {
            var result = _target.GetRecipeIds();

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(_database.MockRecipes.Count);
            foreach (var recipe in _database.MockRecipes)
                result.Should().Contain(recipe.RecipeId);
        }

        [Test]
        public void ListAllRecipes_ReturnsRecipeNames()
        {
            var result = _target.ListAllRecipes();

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(_database.MockRecipes.Count);
            foreach (var recipe in _database.MockRecipes)
                result.Should().Contain(recipe.Name);
        }

        [Test]
        public void GetRecipesInfo_EmptyFilter_ReturnsAllRecipes()
        {
            var result = _target.GetRecipesInfo(new());

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(_database.MockRecipes.Count);
            foreach (var recipe in _database.MockRecipes)
                result.Should().ContainEquivalentOf(recipe with { Ingredients = new(), Instructions = new() });
        }

        [Test]
        public void GetRecipesInfo_FilterByExistingTagInOne_ReturnsOneMatchingRecipe()
        {
            var filter = new RecipeFilterContract { Tags = new() { "breakfast" } };
            var result = _target.GetRecipesInfo(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(_database.MockRecipes[0] with { Ingredients = new(), Instructions = new() });
        }

        [Test]
        public void GetRecipesInfo_FilterByExistingTagInAll_ReturnsOneMatchingRecipe()
        {
            var filter = new RecipeFilterContract { Tags = new() { "simple" } };
            var result = _target.GetRecipesInfo(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(_database.MockRecipes.Count);
            foreach (var recipe in _database.MockRecipes)
                result.Should().ContainEquivalentOf(recipe with { Ingredients = new(), Instructions = new() });
        }

        [Test]
        public void GetRecipesInfo_FilterByNonExistingTag_ReturnsNoRecipes()
        {
            var filter = new RecipeFilterContract { Tags = new() { "dinner" } };
            var result = _target.GetRecipesInfo(filter);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void GetRecipesInfo_FilterByExistingIngredientInOne_ReturnsOneMatchingRecipe()
        {
            var filter = new RecipeFilterContract { Ingredients = new() { "Jelly" } };
            var result = _target.GetRecipesInfo(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(_database.MockRecipes[1] with { Ingredients = new(), Instructions = new() });
        }

        [Test]
        public void GetRecipesInfo_FilterByExistingIngredientInAll_ReturnsOneMatchingRecipe()
        {
            var filter = new RecipeFilterContract { Ingredients = new() { "Milk" } };
            var result = _target.GetRecipesInfo(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(_database.MockRecipes.Count);
            foreach (var recipe in _database.MockRecipes)
                result.Should().ContainEquivalentOf(recipe with { Ingredients = new(), Instructions = new() });
        }

        [Test]
        public void GetRecipesInfo_FilterByNonExistingIngredient_ReturnsNoRecipes()
        {
            var filter = new RecipeFilterContract { Ingredients = new() { "Bread" } };
            var result = _target.GetRecipesInfo(filter);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void GetRecipesInfo_FilterByExistingTagAndIngredientInSameRecipe_ReturnsOneMatchingRecipe()
        {
            var filter = new RecipeFilterContract { Tags = new() { "lunch" }, Ingredients = new() { "Peanut Butter" } };
            var result = _target.GetRecipesInfo(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(_database.MockRecipes[1] with { Ingredients = new(), Instructions = new() });
        }

        [Test]
        public void GetRecipesInfo_FilterByExistingTagAndIngredientInDifferentRecipe_ReturnsNoRecipese()
        {
            var filter = new RecipeFilterContract { Tags = new() { "lunch" }, Ingredients = new() { "Cereal" } };
            var result = _target.GetRecipesInfo(filter);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void GetRecipesFull_EmptyFilter_ReturnsAllRecipesWithIngredientsAndInstructions()
        {
            var result = _target.GetRecipesFull(new());

            result.Should().NotBeNullOrEmpty();
            RecipeVerifier.VerifyRecipeIEnumerable(result, _database.MockRecipes);
        }

        [Test]
        public void GetRecipesFull_FilterByExistingTagInOne_ReturnsOneMatchingRecipeWithIngredientsAndInstructions()
        {
            var filter = new RecipeFilterContract { Tags = new() { "breakfast" } };
            var result = _target.GetRecipesFull(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            RecipeVerifier.VerifyRecipe(result.First(), _database.MockRecipes[0]);
        }

        [Test]
        public void GetRecipesFull_FilterByExistingTagInAll_ReturnsAllMatchingRecipesWithIngredientsAndInstructions()
        {
            var filter = new RecipeFilterContract { Tags = new() { "simple" } };
            var result = _target.GetRecipesFull(filter);

            result.Should().NotBeNullOrEmpty();
            RecipeVerifier.VerifyRecipeIEnumerable(result, _database.MockRecipes);
        }

        [Test]
        public void GetRecipesFull_FilterByNonExistingTag_ReturnsNoRecipes()
        {
            var filter = new RecipeFilterContract { Tags = new() { "dinner" } };
            var result = _target.GetRecipesFull(filter);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void GetRecipesFull_FilterByExistingIngredientInOne_ReturnsOneMatchingRecipeWithIngredientsAndInstructions()
        {
            var filter = new RecipeFilterContract { Ingredients = new() { "Jelly" } };
            var result = _target.GetRecipesFull(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            RecipeVerifier.VerifyRecipe(result.First(), _database.MockRecipes[1]);
        }

        [Test]
        public void GetRecipesFull_FilterByExistingIngredientInAll_ReturnsAllMatchingRecipesWithIngredientsAndInstructions()
        {
            var filter = new RecipeFilterContract { Ingredients = new() { "Milk" } };
            var result = _target.GetRecipesFull(filter);

            result.Should().NotBeNullOrEmpty();
            RecipeVerifier.VerifyRecipeIEnumerable(result, _database.MockRecipes);
        }

        [Test]
        public void GetRecipesFull_FilterByNonExistingIngredient_ReturnsNoRecipes()
        {
            var filter = new RecipeFilterContract { Ingredients = new() { "Bread" } };
            var result = _target.GetRecipesFull(filter);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void GetRecipesFull_FilterByExistingTagAndIngredientInSameRecipe_ReturnsOneMatchingRecipeWithIngredientsAndInstructions()
        {
            var filter = new RecipeFilterContract { Tags = new() { "lunch" }, Ingredients = new() { "Peanut Butter" } };
            var result = _target.GetRecipesFull(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            RecipeVerifier.VerifyRecipe(result.First(), _database.MockRecipes[1]);
        }

        [Test]
        public void GetRecipesFull_FilterByExistingTagAndIngredientInDifferentRecipe_ReturnsNoRecipes()
        {
            var filter = new RecipeFilterContract { Tags = new() { "lunch" }, Ingredients = new() { "Cereal" } };
            var result = _target.GetRecipesFull(filter);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void GetRecipe_ExistingRecipe_ReturnsRecipeWithIngredientsAndInstructions()
        {
            var recipeId = _database.MockRecipes[0].RecipeId;
            var result = _target.GetRecipe(recipeId);

            result.Should().NotBeNull();
            RecipeVerifier.VerifyRecipe(result!, _database.MockRecipes[0]);
        }

        [Test]
        public void GetRecipe_NonExistingRecipe_ReturnsNull()
        {
            var recipeId = Guid.NewGuid();
            var result = _target.GetRecipe(recipeId);

            result.Should().BeNull();
        }

        [Test]
        public void ExportRecipe_ExistingRecipe_ReturnsRecipeContract()
        {
            var recipeId = _database.MockRecipes[0].RecipeId;
            var expected = RecipeContract.FromModel(_database.MockRecipes[0]);
            var result = _target.ExportRecipe(recipeId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ExportRecipe_NonExistingRecipe_ReturnsNull()
        {
            var recipeId = Guid.NewGuid();
            var result = _target.ExportRecipe(recipeId);

            result.Should().BeNull();
        }

        [Test]
        public void ExportRecipes_EmptyFilter_ReturnsAllRecipeContracts()
        {
            var expected = _database.MockRecipes.Select(RecipeContract.FromModel);
            var result = _target.ExportRecipes(new());

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(_database.MockRecipes.Count);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ExportRecipes_FilterByExistingTagInOne_ReturnsOneMatchingRecipeContract()
        {
            var filter = new RecipeFilterContract { Tags = new() { "breakfast" } };
            var expected = _database.MockRecipes.Where(r => r.Tags.Contains("breakfast")).Select(RecipeContract.FromModel);
            var result = _target.ExportRecipes(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ExportRecipes_FilterByExistingTagInAll_ReturnsAllMatchingRecipeContracts()
        {
            var filter = new RecipeFilterContract { Tags = new() { "simple" } };
            var expected = _database.MockRecipes.Select(RecipeContract.FromModel);
            var result = _target.ExportRecipes(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(_database.MockRecipes.Count);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ExportRecipes_FilterByNonExistingTag_ReturnsNoRecipeContracts()
        {
            var filter = new RecipeFilterContract { Tags = new() { "dinner" } };
            var result = _target.ExportRecipes(filter);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void ExportRecipes_FilterByExistingIngredientInOne_ReturnsOneMatchingRecipeContract()
        {
            var filter = new RecipeFilterContract { Ingredients = new() { "Jelly" } };
            var expected = _database.MockRecipes.Where(r => r.Ingredients.Any(i => i.Name == "Jelly")).Select(RecipeContract.FromModel);
            var result = _target.ExportRecipes(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ExportRecipes_FilterByExistingIngredientInAll_ReturnsAllMatchingRecipeContracts()
        {
            var filter = new RecipeFilterContract { Ingredients = new() { "Milk" } };
            var expected = _database.MockRecipes.Select(RecipeContract.FromModel);
            var result = _target.ExportRecipes(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(_database.MockRecipes.Count);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ExportRecipes_FilterByNonExistingIngredient_ReturnsNoRecipeContracts()
        {
            var filter = new RecipeFilterContract { Ingredients = new() { "Bread" } };
            var result = _target.ExportRecipes(filter);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void ExportRecipes_FilterByExistingTagAndIngredientInSameRecipe_ReturnsOneMatchingRecipeContract()
        {
            var filter = new RecipeFilterContract { Tags = new() { "lunch" }, Ingredients = new() { "Peanut Butter" } };
            var expected = _database.MockRecipes.Where(r => r.Tags.Contains("lunch") && r.Ingredients.Any(i => i.Name == "Peanut Butter")).Select(RecipeContract.FromModel);
            var result = _target.ExportRecipes(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ExportRecipes_FilterByExistingTagAndIngredientInDifferentRecipe_ReturnsNoRecipeContracts()
        {
            var filter = new RecipeFilterContract { Tags = new() { "lunch" }, Ingredients = new() { "Cereal" } };
            var result = _target.ExportRecipes(filter);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void ExportRecipes_FilterByExistingTagAndIngredientInAllRecipes_ReturnsAllMatchingRecipeContracts()
        {
            var filter = new RecipeFilterContract { Tags = new() { "simple" }, Ingredients = new() { "Milk" } };
            var expected = _database.MockRecipes.Select(RecipeContract.FromModel);
            var result = _target.ExportRecipes(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(_database.MockRecipes.Count);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void AddRecipe_ReturnsRecipeIdWithExpectedModelInDatabase()
        {
            var recipeContractToAdd = new RecipeContract
            {
                Name = "New Recipe",
                Description = "A new recipe",
                Tags = new() { "new" },
                Servings = 1,
                Ingredients = new()
                {
                    new IngredientContract { Name = "New Ingredient", Quantity = 1, Unit = Units.Cup }
                },
                Instructions = new()
                {
                    new InstructionContract { Name = "New Instruction", Order = 1, Description = "Do something" },
                    new InstructionContract { Name = "New Instruction 2", Order = 2, Description = "Do something more" }
                }
            };

            var resultId = _target.AddRecipe(recipeContractToAdd);
            resultId.Should().NotBeEmpty();

            _database.RecipeContext.Recipes.Count().Should().Be(_database.MockRecipes.Count + 1);
            _database.RecipeContext.Ingredients.Count().Should().Be(_database.MockIngredients.Count + 1);
            _database.RecipeContext.Instructions.Count().Should().Be(_database.MockInstructions.Count + 2);

            var expectedRecipe = recipeContractToAdd.ToModel() with { RecipeId = resultId };
            expectedRecipe.Instructions.ForEach(i => i.RecipeId = resultId);
            expectedRecipe.Ingredients.ForEach(i => i.RecipeId = resultId);

            var resultRecipe = _target.GetRecipe(resultId);
            resultRecipe.Should().NotBeNull();
            RecipeVerifier.VerifyRecipeOnlyEmptyCheckChildrenId(resultRecipe!, expectedRecipe);
        }

        [Test]
        public void DeleteRecipe_ExistingRecipeId_RemovesRecipeFromDatabase()
        {
            var recipeId = _database.MockRecipes[0].RecipeId;
            var deletedId = _target.DeleteRecipe(recipeId);

            deletedId.Should().Be(recipeId);

            _database.RecipeContext.Recipes.Count().Should().Be(_database.MockRecipes.Count - 1);
            _database.RecipeContext.Ingredients.Count().Should().Be(_database.MockIngredients.Count - 2);
            _database.RecipeContext.Instructions.Count().Should().Be(_database.MockInstructions.Count - 2);

            var resultRecipe = _target.GetRecipe(recipeId);
            resultRecipe.Should().BeNull();
        }

        [Test]
        public void DeleteRecipe_NonExistingRecipeId_ReturnsNullAndDatabaseUnchanged()
        {
            var recipeId = Guid.NewGuid();
            var result = _target.DeleteRecipe(recipeId);

            result.Should().BeNull();

            DatabaseVerifier.VerifyUnchangedDatabase(_database);
        }

        [Test]
        public void DeleteRecipes_EmptyList_ReturnsEmptyListAndDatabaseUnchanged()
        {
            var result = _target.DeleteRecipes(new Guid[0]);

            result.Should().NotBeNull();
            result.Should().BeEmpty();

            DatabaseVerifier.VerifyUnchangedDatabase(_database);
        }

        [Test]
        public void DeleteRecipes_ListWithNonExistingRecipeIds_ReturnsEmptyListAndDatabaseUnchanged()
        {
            var result = _target.DeleteRecipes([Guid.NewGuid(), Guid.NewGuid()]);

            result.Should().NotBeNull();
            result.Should().BeEmpty();

            DatabaseVerifier.VerifyUnchangedDatabase(_database);
        }

        [Test]
        public void DeleteRecipes_ListWithAllExistingRecipeIds_ReturnsAllIdsAndRemovesRecipesFromDatabase()
        {
            var recipeIds = _database.MockRecipes.Select(r => r.RecipeId).ToList();
            var result = _target.DeleteRecipes(recipeIds);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(recipeIds);

            _database.RecipeContext.Recipes.Count().Should().Be(0);
            _database.RecipeContext.Ingredients.Count().Should().Be(0);
            _database.RecipeContext.Instructions.Count().Should().Be(0);
        }

        [Test]
        public void DeleteRecipes_ListWithMixedExistingAndNonExistingIds_ReturnsOnlyExistingIdsAndRemovesExistingRecipesFromDatabase()
        {
            var result = _target.DeleteRecipes([Guid.NewGuid(), _database.MockRecipes[1].RecipeId, Guid.NewGuid()]);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.Should().BeEquivalentTo([_database.MockRecipes[1].RecipeId]);

            _database.RecipeContext.Recipes.Count().Should().Be(_database.MockRecipes.Count - 1);
            _database.RecipeContext.Ingredients.Count().Should().Be(_database.MockIngredients.Count - 4);
            _database.RecipeContext.Instructions.Count().Should().Be(_database.MockInstructions.Count - 4);
        }

        [Test]
        public void UpdateRecipe_NonExistingRecipe_ReturnsNullAndDatabaseUnchanged()
        {
            var updateRecipeContract = new UpdateRecipeContract
            {
                RecipeId = Guid.NewGuid(),
                Name = "Updated Recipe",
                Description = "An updated recipe",
                Tags = new() { "updated" },
                Servings = 2,
                Ingredients = new()
                {
                    new UpdateIngredientContract { Name = "Updated Ingredient", Quantity = 2, Unit = Units.Cup }
                },
                Instructions = new()
                {
                    new UpdateInstructionContract { Name = "Updated Instruction", Order = 1, Description = "Do something else" }
                }
            };

            var result = _target.UpdateRecipe(updateRecipeContract);

            result.Should().BeNull();
            DatabaseVerifier.VerifyUnchangedDatabase(_database);
        }

        [Test]
        public void UpdateRecipe_RecipeParametersChanged_ReturnsRecipeIdAndDatabaseUpdated()
        {
            var updateRecipeContract = new UpdateRecipeContract
            {
                RecipeId = _database.MockRecipes[0].RecipeId,
                Name = "Updated Recipe",
                Description = "An updated recipe",
                Tags = new() { "updated" },
                Servings = 2,
                Ingredients = null,
                Instructions = null,
            };

            var result = _target.UpdateRecipe(updateRecipeContract);
            result.Should().Be(updateRecipeContract.RecipeId);

            var expectedRecipe = _database.MockRecipes[0] with
            {
                Name = updateRecipeContract.Name,
                Description = updateRecipeContract.Description,
                Tags = updateRecipeContract.Tags,
                Servings = updateRecipeContract.Servings.Value
            };
            var resultRecipe = _target.GetRecipe(updateRecipeContract.RecipeId);

            RecipeVerifier.VerifyRecipe(resultRecipe!, expectedRecipe);
        }

        [Test]
        public void UpdateRecipe_RecipeIngredientsChanged_ReturnsRecipeIdAndDatabaseUpdated()
        {
            var updateRecipeContract = new UpdateRecipeContract
            {
                RecipeId = _database.MockRecipes[0].RecipeId,
                Ingredients = _database.MockRecipes[0].Ingredients
                            .Select(UpdateIngredientContract.FromModel).ToList()
            };
            updateRecipeContract.Ingredients[0].Name = "Changed ingredient name";

            var result = _target.UpdateRecipe(updateRecipeContract);
            result.Should().Be(updateRecipeContract.RecipeId);

            var expectedRecipe = _database.MockRecipes[0] with { };
            expectedRecipe.Ingredients[0].Name = updateRecipeContract.Ingredients[0].Name!;
            var resultRecipe = _target.GetRecipe(updateRecipeContract.RecipeId);

            RecipeVerifier.VerifyRecipe(resultRecipe!, expectedRecipe);
        }

        [Test]
        public void UpdateRecipe_AddedRecipeIngredient_ReturnsRecipieIdAndIngredientAddedToDatabase()
        {
            var newIngredient = new UpdateIngredientContract
            {
                Name = "New Ingredient",
                Quantity = 1,
                Unit = Units.Cup
            };
            var updateRecipeContract = new UpdateRecipeContract
            {
                RecipeId = _database.MockRecipes[1].RecipeId,
                Ingredients = _database.MockRecipes[1].Ingredients
                            .Select(UpdateIngredientContract.FromModel).ToList()
            };
            updateRecipeContract.Ingredients.Add(newIngredient);

            var result = _target.UpdateRecipe(updateRecipeContract);
            result.Should().Be(updateRecipeContract.RecipeId);

            var expectedRecipe = _database.MockRecipes[1] with { };
            expectedRecipe.Ingredients.Add(newIngredient.ToModel(updateRecipeContract.RecipeId));
            var resultRecipe = _target.GetRecipe(updateRecipeContract.RecipeId);

            RecipeVerifier.VerifyRecipeOnlyEmptyCheckChildrenId(resultRecipe!, expectedRecipe);
        }

        [Test]
        public void UpdateRecipe_RemovedRecipeIngredient_ReturnsRecipieIdAndIngredientRemovedFromDatabase()
        {
            var updateRecipeContract = new UpdateRecipeContract
            {
                RecipeId = _database.MockRecipes[0].RecipeId,
                Ingredients = _database.MockRecipes[0].Ingredients
                            .Select(UpdateIngredientContract.FromModel).ToList()
            };
            updateRecipeContract.Ingredients.RemoveAt(0);

            var result = _target.UpdateRecipe(updateRecipeContract);
            result.Should().Be(updateRecipeContract.RecipeId);

            var expectedRecipe = _database.MockRecipes[0] with { };
            expectedRecipe.Ingredients.RemoveAt(0);
            var resultRecipe = _target.GetRecipe(updateRecipeContract.RecipeId);

            RecipeVerifier.VerifyRecipe(resultRecipe!, expectedRecipe);
        }

        [Test]
        public void UpdateRecipe_RecipeInstructionsChanged_ReturnsRecipeIdAndDatabaseUpdated()
        {
            var updateRecipeContract = new UpdateRecipeContract
            {
                RecipeId = _database.MockRecipes[0].RecipeId,
                Instructions = _database.MockRecipes[0].Instructions
                            .Select(UpdateInstructionContract.FromModel).ToList()
            };
            updateRecipeContract.Instructions[0].Name = "Changed instruction name";

            var result = _target.UpdateRecipe(updateRecipeContract);
            result.Should().Be(updateRecipeContract.RecipeId);

            var expectedRecipe = _database.MockRecipes[0] with { };
            expectedRecipe.Instructions[0].Name = updateRecipeContract.Instructions[0].Name!;
            var resultRecipe = _target.GetRecipe(updateRecipeContract.RecipeId);

            RecipeVerifier.VerifyRecipe(resultRecipe!, expectedRecipe);
        }

        [Test]
        public void UpdateRecipe_AddedRecipeInstruction_ReturnsRecipieIdAndInstructionAddedToDatabase()
        {
            var newInstruction = new UpdateInstructionContract
            {
                Name = "New Instruction",
                Order = 1,
                Description = "Do something"
            };
            var updateRecipeContract = new UpdateRecipeContract
            {
                RecipeId = _database.MockRecipes[1].RecipeId,
                Instructions = _database.MockRecipes[1].Instructions
                            .Select(UpdateInstructionContract.FromModel).ToList()
            };
            updateRecipeContract.Instructions.Add(newInstruction);

            var result = _target.UpdateRecipe(updateRecipeContract);
            result.Should().Be(updateRecipeContract.RecipeId);

            var expectedRecipe = _database.MockRecipes[1] with { };
            expectedRecipe.Instructions.Add(newInstruction.ToModel(updateRecipeContract.RecipeId));
            var resultRecipe = _target.GetRecipe(updateRecipeContract.RecipeId);

            RecipeVerifier.VerifyRecipeOnlyEmptyCheckChildrenId(resultRecipe!, expectedRecipe);
        }

        [Test]
        public void UpdateRecipe_RemovedRecipeInstruction_ReturnsRecipieIdAndInstructionRemovedFromDatabase()
        {
            var updateRecipeContract = new UpdateRecipeContract
            {
                RecipeId = _database.MockRecipes[0].RecipeId,
                Instructions = _database.MockRecipes[0].Instructions
                            .Select(UpdateInstructionContract.FromModel).ToList()
            };
            updateRecipeContract.Instructions.RemoveAt(0);

            var result = _target.UpdateRecipe(updateRecipeContract);
            result.Should().Be(updateRecipeContract.RecipeId);

            var expectedRecipe = _database.MockRecipes[0] with { };
            expectedRecipe.Instructions.RemoveAt(0);
            var resultRecipe = _target.GetRecipe(updateRecipeContract.RecipeId);

            RecipeVerifier.VerifyRecipe(resultRecipe!, expectedRecipe);
        }
    }
}