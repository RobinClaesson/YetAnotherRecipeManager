using RecipeManager.Shared.Models;
using RecipeManager.Web.Models.RecipeStore;

namespace RecipeManager.Web.Store.RecipeStore;

public record RecipesLoadedFromLocalStorageAction(List<Recipe> Recipes);
public record SourceLoadedFromLocalStorageAction(RecipeSource RecipeSource);
public record RecipesFetchedFromSourceAction(RecipeSource RecipeSource, List<Recipe> Recipes);

public record SourceAddedAction(RecipeSource RecipeSource);
public record SourceChangedAction(RecipeSource Original, RecipeSource Updated);
public record SourceRemovedAction(RecipeSource RecipeSource);

public record ResetLocalSourceRecipesAction();
