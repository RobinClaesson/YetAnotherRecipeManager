using Fluxor;
using RecipeManager.Shared.Models;
using RecipeManager.Web.Models.RecipeStore;
using RecipeManager.Web.Store.CommonStore;

namespace RecipeManager.Web.Store.RecipeStore;

public static class RecipeReducers
{
    private static RecipeState AppendLocalStorageCollection(RecipeState state)
    {
        if (!state.RecipieCollections.Any(c => c.Source.Url == Constants.LocalRecipeSourceUrl))
        {
            return state with
            {
                RecipieCollections = state.RecipieCollections.Append(
                                                          new()
                                                          {
                                                              Source = new RecipeSource
                                                              {
                                                                  Name = Constants.LocalRecipeSourceName,
                                                                  Url = Constants.LocalRecipeSourceUrl
                                                              },
                                                          }).ToList()
            };
        }

        return state;
    }

    [ReducerMethod]
    public static RecipeState OnAppLoadedAction(RecipeState state, AppLoadedAction action)
        => AppendLocalStorageCollection(state);

    [ReducerMethod]
    public static RecipeState OnRecipesFetchedFromSourceAction(RecipeState state, RecipesFetchedFromSourceAction action)
    {
        var next = state with { };

        var collection = next.RecipieCollections.First(c => c.Source.Url == action.RecipeSource.Url);
        collection.Recipes.Clear();
        collection.Recipes.AddRange(action.Recipes);

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
        var next = AppendLocalStorageCollection(state);

        var collection = next.RecipieCollections.First(c => c.Source.Url == Constants.LocalRecipeSourceUrl);
        collection.Recipes.Clear();
        collection.Recipes.AddRange(action.Recipes);

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

    [ReducerMethod]
    public static RecipeState OnSourceChangedAction(RecipeState state, SourceChangedAction action)
    {
        var next = state with { };

        var original = next.RecipieCollections.First(c => c.Source.Url == action.Original.Url);
        var updated = original with
        {
            Source = action.Updated
        };
        
        next.RecipieCollections.Remove(original);
        next.RecipieCollections.Add(updated);

        return next;
    }

    [ReducerMethod]
    public static RecipeState OnRecipeAddedAction(RecipeState state, RecipeAddedAction action)
    {
        var next = state with { };

        var collection = next.RecipieCollections.First(c => c.Source.Url == action.Source.Url);
        collection.Recipes.Add(action.Recipe);

        return next;
    }
}
