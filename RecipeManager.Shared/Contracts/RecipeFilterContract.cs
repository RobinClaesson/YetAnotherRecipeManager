namespace RecipeManager.Shared.Contracts;

public record RecipeFilterContract
{
    public List<string> Tags { get; set; } = new();
    public List<string> Ingredients { get; set; } = new();
}
