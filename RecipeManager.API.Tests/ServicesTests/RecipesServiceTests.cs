using FluentAssertions;
using RecipeManager.API.Services;
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
    }
}