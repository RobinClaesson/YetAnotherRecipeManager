using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeManager.API.Services;

namespace RecipeManager.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> AllTags()
        {
            return Ok(_tagService.ListAllTags());
        }
    }
}
