using CommandLine;

namespace RecipeManager.CLI;

public abstract class BaseOptions
{
    [Option('h', "host", Required = true, HelpText = "Reipe Manager API Host")]
    public string? Host { get; set; }

    [Option('p', "port", Required = true, HelpText = "Reipe Manager API Host Port")]
    public int Port { get; set; }

    [Option('s', "use Https", HelpText = "Use HTTPS")]
    public bool UseHttps { get; set; }
}

[Verb("list-recipes", HelpText = "List recipes")]
public class ListRecipesOptions : BaseOptions
{
    [Option('t', "tag", HelpText = "Filter by tag")]
    public string? Tag { get; set; }
}

[Verb("add-recipe", HelpText = "Add a recipe")]
public class AddRecipeOptions : BaseOptions
{

}

[Verb("get-recipes", HelpText = "Get recipe(s)")]
public class GetRecipesOptions : BaseOptions
{
    [Option("id", HelpText = "Recipe ID")]
    public Guid? Id { get; set; }

    [Option('t', "tags", HelpText = "Filter by tags (ignored if --id is set)")]
    public IEnumerable<string>? Tags { get; set; }

    [Option('i', "ingredients", HelpText = "Filter by ingredients (ignored if --id is set)")]
    public IEnumerable<string>? Ingredients { get; set; }
}

[Verb("list-tags", HelpText = "List tags")]
public class ListTagsOptions : BaseOptions
{

}