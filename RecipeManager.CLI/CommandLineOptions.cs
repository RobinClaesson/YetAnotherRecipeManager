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