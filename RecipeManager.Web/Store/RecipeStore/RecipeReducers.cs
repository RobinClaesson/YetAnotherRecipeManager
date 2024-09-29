using Fluxor;
using RecipeManager.Web.Models.RecipeStore;

namespace RecipeManager.Web.Store.RecipeStore;

public static class RecipeReducers
{
    [ReducerMethod]
    public static RecipeState OnRecipesFetchedFromSourceAction(RecipeState state, RecipesFetchedFromSourceAction action)
    {
        var next = state with { };

        next.RecipieCollections.First(c => c.Source.Url == action.RecipeSource.Url)
                            .Recipes.AddRange(action.Recipes);
        return next;
    }

    [ReducerMethod]
    public static RecipeState OnSourceLoadedFromLocalStorageAction(RecipeState state, SourceLoadedFromLocalStorageAction action)
        => state with
        {
            RecipieCollections = state.RecipieCollections.Append(
                                           new()
                                           {
                                               Source = action.RecipeSource,
                                               Recipes = new()
                                           }).ToList()
        };

    [ReducerMethod]
    public static RecipeState OnRecipesLoadedFromLocalStorageAction(RecipeState state, RecipesLoadedFromLocalStorageAction action)
    {
        var next = state with { };
        if (!state.RecipieCollections.Any(c => c.Source.Url == Constants.LocalRecipeSourceUrl))
            next.RecipieCollections.Add(new RecipieCollection
            {
                Source = new RecipeSource
                {
                    Name = "Local",
                    Url = Constants.LocalRecipeSourceUrl
                },
            });

        next.RecipieCollections.First(c => c.Source.Url == Constants.LocalRecipeSourceUrl)
                                .Recipes.AddRange(action.Recipes);
        return next;
    }

    [ReducerMethod]
    public static RecipeState OnSourceAddedAction(RecipeState state, SourceAddedAction action)
        => state with
        {
            RecipieCollections = state.RecipieCollections.Append(
                                       new()
                                       {
                                           Source = action.RecipeSource,
                                           Recipes = new()
                                       }).ToList()
        };

    [ReducerMethod]
    public static RecipeState OnSourceRemovedAction(RecipeState state, SourceRemovedAction action)
        => state with
        {
            RecipieCollections = state.RecipieCollections.Where(c => c.Source.Url != action.RecipeSource.Url).ToList()
        };
}
