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

    [HttpPost]
    public ActionResult<Guid> AddRecipe([FromBody] RecipeContract recipe)
    {
        var postedId = _recipesService.AddRecipe(recipe);
        return Ok(postedId);
    }
}
