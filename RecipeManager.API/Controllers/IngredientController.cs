using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeManager.API.Services;

namespace RecipeManager.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientsService _ingredientsService;

        public IngredientController(IIngredientsService ingredientsService)
        {
            _ingredientsService = ingredientsService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> IngredientNames()
        {
            return Ok(_ingredientsService.ListAllIngredients());
        }
    }
}
