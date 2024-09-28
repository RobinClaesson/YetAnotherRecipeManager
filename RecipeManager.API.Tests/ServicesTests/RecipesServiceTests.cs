using FluentAssertions;
using RecipeManager.API.Services;
using RecipeManager.Shared.Db;

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

    }
}