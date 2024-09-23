using CommandLine;
using RecipeManager.CLI;
using System.Net.Http.Json;

await Parser.Default.ParseArguments<ListRecipesOptions>(args)
    .MapResult(
        (ListRecipesOptions options) => ListRecipes(options),
        HandleParseError
    );
    

async Task HandleParseError(IEnumerable<Error> errors)
{
    foreach (var error in errors)
    {
        Console.WriteLine(error);
    }
}

async Task ListRecipes(ListRecipesOptions options)
{
    Console.WriteLine($"Fetching recipes from {options.Host}:{options.Port}");

    using var client = new HttpClient();
    client.BaseAddress = new UriBuilder
    {
        Scheme = options.UseHttps ? "https" : "http",
        Host = options.Host,
        Port = options.Port
    }.Uri;

    var result = await client.GetFromJsonAsync<List<string>>("/api/Recipe/RecipeNames");

    if(result == null)
    {
        Console.WriteLine("Could not fetch recipes...");
        return;
    }

    if(result.Count == 0)
    {
        Console.WriteLine("No recipes found");
        return;
    }

    Console.WriteLine("Recipes:");
    foreach (var recipe in result)
    {
        Console.WriteLine(recipe);
    }
}