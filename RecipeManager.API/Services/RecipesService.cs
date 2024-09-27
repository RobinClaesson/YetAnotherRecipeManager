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
    public IEnumerable<RecipeContract> ExportRecipes(RecipeFilterContract filter);
    public Recipe? GetRecipe(Guid recipeId);
    public RecipeContract? ExportRecipe(Guid recipeId);
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
            recipes = recipes.Where(r => filter.Tags.All(ft => r.Tags.Contains(ft)));

        if(filter.Ingredients.Any())
            recipes = recipes.Where(r => filter.Ingredients.All(fi => r.Ingredients.Any(ri => ri.Name == fi)));

        return recipes;
    }

    public IEnumerable<Recipe> GetRecipesFull(RecipeFilterContract filter)
    {
        var recipes = _recipeContext.Recipes.AsQueryable();

        if (filter.Tags.Any())
            recipes = recipes.Where(r => filter.Tags.All(ft => r.Tags.Contains(ft)));

        if (filter.Ingredients.Any())
            recipes = recipes.Where(r => filter.Ingredients.All(fi => r.Ingredients.Any(ri => ri.Name == fi)));

        return recipes.Include(r => r.Ingredients).Include(r => r.Instructions);
    }

    public Recipe? GetRecipe(Guid recipeId)
    {
        return _recipeContext.Recipes
                            .Include(r => r.Ingredients)
                            .Include(r => r.Instructions)
                            .FirstOrDefault(r => r.RecipeId == recipeId);
    }

    public RecipeContract? ExportRecipe(Guid recipeId)
    {
        var recipe = GetRecipe(recipeId);
        return recipe is null ? null : RecipeContract.FromModel(recipe);
    }

    public IEnumerable<RecipeContract> ExportRecipes(RecipeFilterContract filter)
    {
        var recipes = GetRecipesFull(filter);
        return recipes.Select(RecipeContract.FromModel);
    }

    public Guid AddRecipe(RecipeContract recipeContract)
    {
        var recipe = recipeContract.ToModel();
        var posted = _recipeContext.Recipes.Add(recipe);
        _recipeContext.SaveChanges();
        return posted.Entity.RecipeId;
    }
}
