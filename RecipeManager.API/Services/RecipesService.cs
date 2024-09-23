using RecipeManager.Shared.Contracts;
using RecipeManager.Shared.Db;
using RecipeManager.Shared.Models;

namespace RecipeManager.API.Services;

public interface IRecipesService
{
    public IEnumerable<string> ListAllRecipes();
    public Recipe AddRecipe(RecipeContract recipe);
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

    public Recipe AddRecipe(RecipeContract recipeContract)
    {
        var recipe = recipeContract.ToModel();
        var posted = _recipeContext.Recipes.Add(recipe);
        _recipeContext.SaveChanges();
        return posted.Entity;
    }
}
