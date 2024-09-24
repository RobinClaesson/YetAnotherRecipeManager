using Fluxor;

namespace RecipeManager.Web.Store.RecipeStore;

public static class RecipeReducers
{
    [ReducerMethod]
    public static RecipeState OnRecipesFetchedFromSourceAction(RecipeState state, RecipesFetchedFromSourceAction action)
        => state with
        {
            RecipieCollections = state.RecipieCollections.Append(
                                        new()
                                        {
                                            Source = action.RecipeSource,
                                            Recipes = action.Recipes
                                        }).ToList()
        };
}
