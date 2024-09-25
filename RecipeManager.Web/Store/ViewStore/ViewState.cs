using Fluxor;

namespace RecipeManager.Web.Store.ViewStore;

[FeatureState]
public record ViewState
{
    public bool DarkMode { get; init; }
}
