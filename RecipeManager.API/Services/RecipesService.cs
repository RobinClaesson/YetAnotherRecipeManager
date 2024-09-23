using RecipeManager.Shared.Db;

namespace RecipeManager.API.Services;

public interface IRecipesService
{
    public IEnumerable<string> ListAllRecipes();
}

public class RecipesService : IRecipesService
{
    private readonly RecipeContext _recipeContext;

    public RecipesService(RecipeContext recipeContext)
    {
        _recipeContext = recipeContext;
        _recipeContext.Database.EnsureCreated();
    }

    public IEnumerable<string> ListAllRecipes()
    {
        return _recipeContext.Recipes.Select(r => r.Name).OrderBy(s => s);
    }
}
