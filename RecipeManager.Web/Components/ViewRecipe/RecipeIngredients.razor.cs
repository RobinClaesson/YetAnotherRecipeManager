using Microsoft.AspNetCore.Components;
using RecipeManager.Shared.Models;

namespace RecipeManager.Web.Components.ViewRecipe;

public partial class RecipeIngredients
{
    [Parameter]
    public IEnumerable<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    [Parameter]
    public int RecipeServings { get; set; } = 1;

    private int _selectedServings = 1;
    private double ServingRatio => (double)_selectedServings / (double)RecipeServings;

    override protected void OnParametersSet()
    {
        _selectedServings = RecipeServings;
        base.OnParametersSet();
    }
}
