using CommandLine;
using RecipeManager.CLI;
using RecipeManager.Shared.Contracts;
using RecipeManager.Shared.Models;
using System.Net.Http.Json;

await Parser.Default.ParseArguments<ListRecipesOptions, AddRecipeOptions>(args)
    .MapResult(
        (ListRecipesOptions options) => ListRecipes(options),
        (AddRecipeOptions options) => AddRecipe(options),
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

    using var client = HttpClientFactory.GetClient(options);

    var result = await client.GetFromJsonAsync<List<string>>("/api/Recipe/RecipeNames");

    if (result == null)
    {
        Console.WriteLine("Could not fetch recipes...");
        return;
    }

    if (result.Count == 0)
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

async Task AddRecipe(AddRecipeOptions options)
{
    var recipe = new RecipeContract();

    Console.WriteLine("Enter Recipe Name: ");
    string? name = null;
    while (string.IsNullOrEmpty(name))
        name = Console.ReadLine();
    recipe.Name = name.Trim();

    Console.WriteLine("Enter Recipe Description: ");
    string? description = null;
    while (string.IsNullOrEmpty(description))
        description = Console.ReadLine();
    recipe.Description = description.Trim();

    Console.WriteLine("Enter Recipe Tags, end with empty line: ");
    string? tag = null;
    while (!string.IsNullOrEmpty(tag = Console.ReadLine()))
        recipe.Tags.Add(tag.Trim());

    Console.WriteLine("Enter Recipe Servings: ");
    int servings = 0;
    while (!int.TryParse(Console.ReadLine(), out servings))
        Console.WriteLine("Invalid input, please enter a number");
    recipe.Servings = servings;

    Console.WriteLine("Enter Recipe Ingredients (ingredient quantity unit), end with empty line: ");
    string? ingredient = null;
    while (!string.IsNullOrEmpty(ingredient = Console.ReadLine()))
    {
        var parts = ingredient.Split(' ');
        if (parts.Length != 3)
        {
            Console.WriteLine("Invalid input, please enter ingredient and unit. Ex: Milk 1.5 dl");
            continue;
        }

        if (!double.TryParse(parts[1], out double quantity))
        {
            Console.WriteLine("Invalid quantity.");
            continue;
        }

        if (!UnitsParser.TryParse(parts[2], out Units unit))
        {
            Console.WriteLine("Unknwn unit.");
            continue;
        }

        recipe.Ingredients.Add(new IngredientContract
        {
            Name = parts[0].Trim(),
            Unit = unit,
            Quantity = quantity
        });
    }

    Console.WriteLine("Enter Recipe Instructions (name:instruction), end with empty line: ");
    string? instruction = null;
    while (!string.IsNullOrEmpty(instruction = Console.ReadLine()))
    {
        var parts = instruction.Split(':');
        if (parts.Length != 2)
        {
            Console.WriteLine("Invalid input, please enter instruction. Ex: Step 1: Do something");
            continue;
        }

        recipe.Instructions.Add(new InstructionContract
        {
            Order = recipe.Instructions.Count + 1,
            Name = parts[0].Trim(),
            Description = parts[1].Trim()
        });
    }

    Console.WriteLine("Recipe Complete:");
    Console.WriteLine(recipe);

    Console.WriteLine("Do you want to save this recipe? (y/n)");
    char save = ' ';
    while ((save = (Console.ReadLine() ?? string.Empty).FirstOrDefault()) != 'y' && save != 'n')
        Console.WriteLine("Do you want to save this recipe? (y/n)");

    if (save == 'n')
    {
        Console.WriteLine("Recipe not saved.");
        return;
    }

    Console.WriteLine($"Saving Recipes to {options.Host}:{options.Port}");
    using var client = HttpClientFactory.GetClient(options);

    var response = await client.PostAsJsonAsync("/api/Recipe/AddRecipe", recipe);

    if(response.IsSuccessStatusCode)
    {
        var posted = await response.Content.ReadFromJsonAsync<Recipe>();
        if(posted == null)
        {
            Console.WriteLine("Failed to save recipe.");
            return;
        }
        Console.WriteLine($"Recipe ${posted.Name} saved!");
    }
    else
    {
        Console.WriteLine("Failed to save recipe.");
        Console.WriteLine(response.ReasonPhrase);
    }
}