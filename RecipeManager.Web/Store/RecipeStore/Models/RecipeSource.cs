namespace RecipeManager.Web.Store.RecipeStore.Models;

public record RecipeSource
{
    public string Name { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
}
