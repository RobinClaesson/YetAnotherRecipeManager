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

    [HttpPost]
    public ActionResult<Recipe> AddRecipe([FromBody] RecipeContract recipe)
    {
        var posted = _recipesService.AddRecipe(recipe);
        return Ok(posted);
    }
}
