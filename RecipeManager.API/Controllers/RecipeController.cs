using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeManager.API.Services;
using RecipeManager.Shared.Contracts;
using RecipeManager.Shared.Models;

namespace RecipeManager.API.Controllers;

[Route("api/[controller]/[Action]")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly IRecipesService _recipesService;

    public RecipeController(IRecipesService recipesService)
    {
        _recipesService = recipesService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Guid>> RecipeIds()
    {
        return Ok(_recipesService.GetRecipeIds());
    }

    [HttpGet]
    public ActionResult<IEnumerable<string>> RecipeNames()
    {
        return Ok(_recipesService.ListAllRecipes());
    }

    [HttpGet]
    public ActionResult<IEnumerable<Recipe>> GetRecipesInfo([FromQuery] RecipeFilterContract filter)
    {
        return Ok(_recipesService.GetRecipesInfo(filter));
    }

    [HttpGet]
    public ActionResult<IEnumerable<Recipe>> GetRecipesFull([FromQuery] RecipeFilterContract filter)
    {
        return Ok(_recipesService.GetRecipesFull(filter));
    }

    [HttpGet]
    public ActionResult<Recipe> GetRecipe(Guid recipeId)
    {
        var recipe = _recipesService.GetRecipe(recipeId);
        if (recipe is null)
            return NotFound();
        return Ok(recipe);
    }

    [HttpGet]
    public ActionResult<RecipeContract> ExportRecipe(Guid recipeId)
    {
        var recipe = _recipesService.ExportRecipe(recipeId);
        if (recipe is null)
            return NotFound();
        return Ok(recipe);
    }

    [HttpGet]
    public ActionResult<IEnumerable<RecipeContract>> ExportRecipes([FromQuery] RecipeFilterContract filter)
    {
        return Ok(_recipesService.ExportRecipes(filter));
    }

    [HttpPost]
    public ActionResult<Guid> AddRecipe([FromBody] RecipeContract recipe)
    {
        var postedId = _recipesService.AddRecipe(recipe);
        return Ok(postedId);
    }

    [HttpPost]
    public ActionResult<IEnumerable<Guid>> AddRecipes([FromBody] IEnumerable<RecipeContract> recipes)
    {
        var postedIds = _recipesService.AddRecipes(recipes);
        return Ok(postedIds);
    }
}
