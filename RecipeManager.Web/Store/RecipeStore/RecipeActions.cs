using RecipeManager.Shared.Models;
using RecipeManager.Web.Store.RecipeStore.Models;

namespace RecipeManager.Web.Store.RecipeStore;

public record RecipesFetchedFromSourceAction(RecipeSource RecipeSource, List<Recipe> Recipes);
