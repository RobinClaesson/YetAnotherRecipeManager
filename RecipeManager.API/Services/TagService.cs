using RecipeManager.Shared.Db;

namespace RecipeManager.API.Services;

public interface ITagService
{
    public IEnumerable<string> ListAllTags();
}

public class TagService : ITagService
{
    private readonly RecipeContext _recipeContext;

    public TagService(RecipeContext recipeContext)
    {
        _recipeContext = recipeContext;
        _recipeContext.Database.EnsureCreated();
    }

    public IEnumerable<string> ListAllTags()
    {
        return _recipeContext.Recipes.AsEnumerable().SelectMany(r => r.Tags).Distinct().OrderBy(s => s);
    }
}
