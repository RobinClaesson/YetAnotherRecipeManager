using FluentAssertions;
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
        private RecipeContext _recipeContext;

        [SetUp]
        public void Setup()
        {
            _recipeContext = MockDatabase.DatabaseSetup();
            _target = new RecipesService(_recipeContext);
        }

        [TearDown]
        public void TearDown()
        {
            MockDatabase.DatabaseTeardown(_recipeContext);
            _recipeContext.Dispose();
        }

        [Test]
        public void GetRecipeIds_ReturnsRecipeIds()
        {
            var result = _target.GetRecipeIds();

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(MockDatabase.MockRecipes.Count);
            foreach (var recipe in MockDatabase.MockRecipes)
                result.Should().Contain(recipe.RecipeId);
        }

        [Test]
        public void ListAllRecipes_ReturnsRecipeNames()
        {
            var result = _target.ListAllRecipes();

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(MockDatabase.MockRecipes.Count);
            foreach (var recipe in MockDatabase.MockRecipes)
                result.Should().Contain(recipe.Name);
        }

        [Test]
        public void GetRecipesInfo_EmptyFilter_ReturnsAllRecipes()
        {
            var result = _target.GetRecipesInfo(new());

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(MockDatabase.MockRecipes.Count);
            foreach (var recipe in MockDatabase.MockRecipes)
                result.Should().ContainEquivalentOf(recipe with { Ingredients = new(), Instructions = new() });
        }

        [Test]
        public void GetRecipesInfo_FilterByExistingTagInOne_ReturnsOneMatchingRecipe()
        {
            var filter = new RecipeFilterContract { Tags = new() { "breakfast" } };
            var result = _target.GetRecipesInfo(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(MockDatabase.MockRecipes[0] with { Ingredients = new(), Instructions = new() });
        }

        [Test]
        public void GetRecipesInfo_FilterByExistingTagInAll_ReturnsOneMatchingRecipe()
        {
            var filter = new RecipeFilterContract { Tags = new() { "simple" } };
            var result = _target.GetRecipesInfo(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(MockDatabase.MockRecipes.Count);
            foreach (var recipe in MockDatabase.MockRecipes)
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
            result.Should().ContainEquivalentOf(MockDatabase.MockRecipes[1] with { Ingredients = new(), Instructions = new() });
        }

        [Test]
        public void GetRecipesInfo_FilterByExistingIngredientInAll_ReturnsOneMatchingRecipe()
        {
            var filter = new RecipeFilterContract { Ingredients = new() { "Milk" } };
            var result = _target.GetRecipesInfo(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(MockDatabase.MockRecipes.Count);
            foreach (var recipe in MockDatabase.MockRecipes)
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
            result.Should().ContainEquivalentOf(MockDatabase.MockRecipes[1] with { Ingredients = new(), Instructions = new() });
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
            RecipeVerifier.VerifyRecipeIEnumerable(result, MockDatabase.MockRecipes);
        }

        [Test]
        public void GetRecipesFull_FilterByExistingTagInOne_ReturnsOneMatchingRecipeWithIngredientsAndInstructions()
        {
            var filter = new RecipeFilterContract { Tags = new() { "breakfast" } };
            var result = _target.GetRecipesFull(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            RecipeVerifier.VerifyRecipe(result.First(), MockDatabase.MockRecipes[0]);
        }

        [Test]
        public void GetRecipesFull_FilterByExistingTagInAll_ReturnsAllMatchingRecipesWithIngredientsAndInstructions()
        {
            var filter = new RecipeFilterContract { Tags = new() { "simple" } };
            var result = _target.GetRecipesFull(filter);

            result.Should().NotBeNullOrEmpty();
            RecipeVerifier.VerifyRecipeIEnumerable(result, MockDatabase.MockRecipes);
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
            RecipeVerifier.VerifyRecipe(result.First(), MockDatabase.MockRecipes[1]);
        }

        [Test]
        public void GetRecipesFull_FilterByExistingIngredientInAll_ReturnsAllMatchingRecipesWithIngredientsAndInstructions()
        {
            var filter = new RecipeFilterContract { Ingredients = new() { "Milk" } };
            var result = _target.GetRecipesFull(filter);

            result.Should().NotBeNullOrEmpty();
            RecipeVerifier.VerifyRecipeIEnumerable(result, MockDatabase.MockRecipes);
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
            RecipeVerifier.VerifyRecipe(result.First(), MockDatabase.MockRecipes[1]);
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
            var recipeId = MockDatabase.MockRecipes[0].RecipeId;
            var result = _target.GetRecipe(recipeId);

            result.Should().NotBeNull();
            RecipeVerifier.VerifyRecipe(result!, MockDatabase.MockRecipes[0]);
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
            var recipeId = MockDatabase.MockRecipes[0].RecipeId;
            var expected = RecipeContract.FromModel(MockDatabase.MockRecipes[0]);
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
            var expected = MockDatabase.MockRecipes.Select(RecipeContract.FromModel);
            var result = _target.ExportRecipes(new());

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(MockDatabase.MockRecipes.Count);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ExportRecipes_FilterByExistingTagInOne_ReturnsOneMatchingRecipeContract()
        {
            var filter = new RecipeFilterContract { Tags = new() { "breakfast" } };
            var expected = MockDatabase.MockRecipes.Where(r => r.Tags.Contains("breakfast")).Select(RecipeContract.FromModel);
            var result = _target.ExportRecipes(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ExportRecipes_FilterByExistingTagInAll_ReturnsAllMatchingRecipeContracts()
        {
            var filter = new RecipeFilterContract { Tags = new() { "simple" } };
            var expected = MockDatabase.MockRecipes.Select(RecipeContract.FromModel);
            var result = _target.ExportRecipes(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(MockDatabase.MockRecipes.Count);
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
            var expected = MockDatabase.MockRecipes.Where(r => r.Ingredients.Any(i => i.Name == "Jelly")).Select(RecipeContract.FromModel);
            var result = _target.ExportRecipes(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ExportRecipes_FilterByExistingIngredientInAll_ReturnsAllMatchingRecipeContracts()
        {
            var filter = new RecipeFilterContract { Ingredients = new() { "Milk" } };
            var expected = MockDatabase.MockRecipes.Select(RecipeContract.FromModel);
            var result = _target.ExportRecipes(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(MockDatabase.MockRecipes.Count);
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
            var expected = MockDatabase.MockRecipes.Where(r => r.Tags.Contains("lunch") && r.Ingredients.Any(i => i.Name == "Peanut Butter")).Select(RecipeContract.FromModel);
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
            var expected = MockDatabase.MockRecipes.Select(RecipeContract.FromModel);
            var result = _target.ExportRecipes(filter);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(MockDatabase.MockRecipes.Count);
            result.Should().BeEquivalentTo(expected);
        }
    }
}