using Microsoft.AspNetCore.Components;
using RecipeManager.Shared.Models;

namespace RecipeManager.Web.Components.ViewRecipe;

public partial class RecipeInstructions
{
    [Parameter]
    public IEnumerable<Instruction> Instructions { get; set; } = new List<Instruction>();
}
