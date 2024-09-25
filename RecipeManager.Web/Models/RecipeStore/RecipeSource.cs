namespace RecipeManager.Web.Models.RecipeStore;

public record RecipeSource
{
    public string Name { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
}
