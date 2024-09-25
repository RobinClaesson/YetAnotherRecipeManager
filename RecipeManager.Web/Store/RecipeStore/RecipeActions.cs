using RecipeManager.Shared.Models;
using RecipeManager.Web.Models.RecipeStore;

namespace RecipeManager.Web.Store.RecipeStore;

public record RecipesFetchedFromSourceAction(RecipeSource RecipeSource, List<Recipe> Recipes);
