using Microsoft.EntityFrameworkCore;
using RecipeManager.Shared.Contracts;
using RecipeManager.Shared.Db;
using RecipeManager.Shared.Models;

namespace RecipeManager.API.Services;

public interface IRecipesService
{
    public IEnumerable<string> ListAllRecipes();
    public IEnumerable<Recipe> GetRecipesInfo(RecipeFilterContract filter);
    public IEnumerable<Recipe> GetRecipesFull(RecipeFilterContract filter);
    public Recipe? GetRecipe(Guid recipeId);
    public Guid AddRecipe(RecipeContract recipe);
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

    public IEnumerable<Recipe> GetRecipesInfo(RecipeFilterContract filter)
    {
        var recipes = _recipeContext.Recipes.AsQueryable();

        if (filter.Tags.Any())
            recipes = recipes.Where(r => r.Tags.Any(t => filter.Tags.Contains(t)));

        if(filter.Ingredients.Any())
            recipes = recipes.Where(r => r.Ingredients.Any(i => filter.Ingredients.Contains(i.Name)));

        return recipes;
    }

    public IEnumerable<Recipe> GetRecipesFull(RecipeFilterContract filter)
    {
        var recipes = _recipeContext.Recipes.AsQueryable();

        if (filter.Tags.Any())
            recipes = recipes.Where(r => r.Tags.Any(t => filter.Tags.Contains(t)));

        if (filter.Ingredients.Any())
            recipes = recipes.Where(r => r.Ingredients.Any(i => filter.Ingredients.Contains(i.Name)));

        return recipes.Include(r => r.Ingredients).Include(r => r.Instructions);
    }

    public Recipe? GetRecipe(Guid recipeId)
    {
        if(_recipeContext.Recipes.Find(recipeId) is Recipe recipe)
            return recipe;

        return null;
    }

    public Guid AddRecipe(RecipeContract recipeContract)
    {
        var recipe = recipeContract.ToModel();
        var posted = _recipeContext.Recipes.Add(recipe);
        _recipeContext.SaveChanges();
        return posted.Entity.RecipeId;
    }
}
