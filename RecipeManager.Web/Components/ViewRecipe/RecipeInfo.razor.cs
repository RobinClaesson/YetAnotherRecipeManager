using Microsoft.AspNetCore.Components;
using RecipeManager.Shared.Models;

namespace RecipeManager.Web.Components.ViewRecipe;

public partial class RecipeInfo
{
    [Parameter]
    public Recipe Recipe { get; set; } = new Recipe();
}
