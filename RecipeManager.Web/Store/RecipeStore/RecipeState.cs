using Fluxor;
using RecipeManager.Web.Models.RecipeStore;

namespace RecipeManager.Web.Store.RecipeStore;

[FeatureState]
public record RecipeState
{
    public List<RecipieCollection> RecipieCollections { get; init; } = new List<RecipieCollection>();
}
