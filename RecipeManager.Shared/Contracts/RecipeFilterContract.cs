namespace RecipeManager.Shared.Contracts;

public record RecipeFilterContract
{
    public List<string> Tags { get; set; } = new();
    public List<string> Ingredients { get; set; } = new();

    public string ToQueryString()
    {
        var tags = Tags.Select(t => $"tags={t}");
        var ingredients = Ingredients.Select(i => $"ingredients={i}");

        return string.Join('&', tags.Concat(ingredients));
    }
}
