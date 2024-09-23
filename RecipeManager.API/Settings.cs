using RecipeManager.Shared;

namespace RecipeManager.API;

public static class Settings
{
    public static string DbPath { get; set; } = Defaults.DbPath;
}
