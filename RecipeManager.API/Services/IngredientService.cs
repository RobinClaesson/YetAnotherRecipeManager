using RecipeManager.Shared.Db;

namespace RecipeManager.API.Services;

public interface IIngredientsService
{
    public IEnumerable<string> ListAllIngredients();
}

public class IngredientService : IIngredientsService
{
    private readonly RecipeContext _recipeContext;

    public IngredientService(RecipeContext recipeContext)
    {
        _recipeContext = recipeContext;
        _recipeContext.Database.EnsureCreated();
    }

    public IEnumerable<string> ListAllIngredients()
    {
        //return _recipeContext.Recipes.SelectMany(r => r.Ingredients)
        //                                .Select(i => i.Name)
        //                                .Distinct()
        //                                .OrderBy(n => n);

        return _recipeContext.Ingredients.Select(i => i.Name).Distinct().OrderBy(n => n);
    }
}
