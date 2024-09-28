using Microsoft.EntityFrameworkCore;
using RecipeManager.Shared.Contracts;
using RecipeManager.Shared.Db;
using RecipeManager.Shared.Models;

namespace RecipeManager.API.Services;

public interface IRecipesService
{
    public IEnumerable<Guid> GetRecipeIds();
    public IEnumerable<string> ListAllRecipes();
    public IEnumerable<Recipe> GetRecipesInfo(RecipeFilterContract filter);
    public IEnumerable<Recipe> GetRecipesFull(RecipeFilterContract filter);
    public IEnumerable<RecipeContract> ExportRecipes(RecipeFilterContract filter);
    public Recipe? GetRecipe(Guid recipeId);
    public RecipeContract? ExportRecipe(Guid recipeId);
    public Guid AddRecipe(RecipeContract recipe);
    public IEnumerable<Guid> AddRecipes(IEnumerable<RecipeContract> recipes);
    public Guid? DeleteRecipe(Guid recipeId);
    public IEnumerable<Guid> DeleteRecipes(IEnumerable<Guid> recipeIds);
    public Guid? UpdateRecipe(UpdateRecipeContract updateRecipeContract);
}

public class RecipesService : IRecipesService
{
    private readonly RecipeContext _recipeContext;

    public RecipesService(RecipeContext recipeContext)
    {
        _recipeContext = recipeContext;
        _recipeContext.Database.EnsureCreated();
    }

    public IEnumerable<Guid> GetRecipeIds()
    {
        return _recipeContext.Recipes.Select(r => r.RecipeId);
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

        if (filter.Ingredients.Any())
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

    public IEnumerable<Guid> AddRecipes(IEnumerable<RecipeContract> recipes)
    {
        var recipeModels = recipes.Select(r => r.ToModel());
        var guids = new List<Guid>();

        foreach (var recipe in recipeModels)
        {
            var posted = _recipeContext.Recipes.Add(recipe);
            guids.Add(posted.Entity.RecipeId);
        }

        _recipeContext.SaveChanges();
        return guids;
    }

    public Guid? DeleteRecipe(Guid recipeId)
    {
        if (!_recipeContext.Recipes.Any(r => r.RecipeId == recipeId))
            return null;

        var recipe = _recipeContext.Recipes.Find(recipeId);
        _recipeContext.Recipes.Remove(recipe!);
        _recipeContext.SaveChanges();
        return recipeId;
    }

    public IEnumerable<Guid> DeleteRecipes(IEnumerable<Guid> recipeIds)
    {
        var recipes = _recipeContext.Recipes.Where(r => recipeIds.Contains(r.RecipeId)).ToList();
        _recipeContext.Recipes.RemoveRange(recipes);
        _recipeContext.SaveChanges();
        return recipes.Select(r => r.RecipeId);
    }

    public Guid? UpdateRecipe(UpdateRecipeContract updateRecipeContract)
    {
        if (!_recipeContext.Recipes.Any(r => r.RecipeId == updateRecipeContract.RecipeId))
            return null;

        //Just calling Update on the Recipe will not update the children but create new ones
        //So we need to manually update each part of the Recipe

        //Recipe
        var dbRecipe = _recipeContext.Recipes.Find(updateRecipeContract.RecipeId)!;
        if (updateRecipeContract.Name is string recipeName)
            dbRecipe.Name = recipeName;
        if (updateRecipeContract.Description is string recipeDescription)
            dbRecipe.Description = recipeDescription;
        if (updateRecipeContract.Servings is int recipeServings)
            dbRecipe.Servings = recipeServings;
        if (updateRecipeContract.Tags is List<string> recipeTags)
            dbRecipe.Tags = recipeTags;

        //Ingredients
        if (updateRecipeContract.Ingredients is IEnumerable<UpdateIngredientContract> updatedIngredients)
        {
            var dbIngredients = _recipeContext.Ingredients.Where(i => i.RecipeId == updateRecipeContract.RecipeId).ToList();
            foreach (var updateIngredient in updatedIngredients)
            {
                if (updateIngredient.IngredientId is Guid ingredientId
                    && dbIngredients.FirstOrDefault(i => i.IngredientId == ingredientId) is Ingredient dbIngredient)
                {
                    if (updateIngredient.Name is string ingredientName)
                        dbIngredient.Name = ingredientName;
                    if (updateIngredient.Quantity is double ingredientQuantity)
                        dbIngredient.Quantity = ingredientQuantity;
                    if (updateIngredient.Unit is Units ingredientUnit)
                        dbIngredient.Unit = ingredientUnit;
                }
                else
                {
                    var ingredient = new Ingredient
                    {
                        Name = updateIngredient.Name ?? string.Empty,
                        Quantity = updateIngredient.Quantity ?? 0,
                        Unit = updateIngredient.Unit ?? Units.None,
                        RecipeId = dbRecipe.RecipeId,
                    };
                    _recipeContext.Ingredients.Add(ingredient);
                }
            }

            var deletedIngredients = dbIngredients.Where(i => !updatedIngredients.Any(ui => ui.IngredientId == i.IngredientId)).ToList();
            _recipeContext.Ingredients.RemoveRange(deletedIngredients);
        }

        //Instructions
        if (updateRecipeContract.Instructions is IEnumerable<UpdateInstructionContract> updatedInstructions)
        {
            var dbInstructions = _recipeContext.Instructions.Where(i => i.RecipeId == updateRecipeContract.RecipeId).ToList();
            foreach (var updateInstruction in updatedInstructions)
            {
                if (updateInstruction.InstructionId is Guid instructionId
                     && dbInstructions.FirstOrDefault(i => i.InstructionId == instructionId) is Instruction dbInstruction)
                {
                    if( updateInstruction.Name is string instructionName)
                        dbInstruction.Name = instructionName;
                    if (updateInstruction.Order is int instructionOrder)
                        dbInstruction.Order = instructionOrder;
                    if (updateInstruction.Description is string instructionDescription)
                        dbInstruction.Description = instructionDescription;
                }
                else
                {
                    var instruction = new Instruction
                    {
                        Name = updateInstruction.Name ?? string.Empty,
                        Order = updateInstruction.Order ?? 0,
                        Description = updateInstruction.Description ?? string.Empty,
                        RecipeId = dbRecipe.RecipeId,
                    };
                    _recipeContext.Instructions.Add(instruction);
                }
            }

            var deletedInstructions = dbInstructions.Where(i => !updatedInstructions.Any(ui => ui.InstructionId == i.InstructionId)).ToList();
            _recipeContext.Instructions.RemoveRange(deletedInstructions);
        }

        _recipeContext.SaveChanges();
        return updateRecipeContract.RecipeId;
    }

}
