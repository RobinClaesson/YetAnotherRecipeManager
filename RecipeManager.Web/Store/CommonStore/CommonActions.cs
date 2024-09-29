namespace RecipeManager.Web.Store.CommonStore;

public record AppLoadedAction();

public record ErrorOccurredAction(string ErrorMessage);