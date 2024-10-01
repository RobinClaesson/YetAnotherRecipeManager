using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MudBlazor;
using RecipeManager.Shared.Contracts;
using RecipeManager.Shared.Models;
using RecipeManager.Web.Models.RecipeStore;
using RecipeManager.Web.Store.RecipeStore;

namespace RecipeManager.Web.Components.AddRecipe;

public partial class AddRecipeForm
{
    [Inject]
    IState<RecipeState> RecipeState { get; set; } = default!;

    [Inject]
    IDispatcher Dispatcher { get; set; } = default!;

    [Parameter]
    public RecipeContract RecipeContract { get; set; } = new() { Servings = 4 };

    public RecipeSource RecipeSource { get; set; } = new() { Name = Constants.LocalRecipeSourceName, Url = Constants.LocalRecipeSourceUrl };

    private IEnumerable<RecipeSource> RecipeSources
        => RecipeState.Value.RecipieCollections.Select(x => x.Source);

    private IEnumerable<Units> AllUnits => Enum.GetValues<Units>();

    private string _currentTagEntry = string.Empty;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        RecipeSource = RecipeState.Value.RecipieCollections.Select(x => x.Source).FirstOrDefault() ?? RecipeSource;
    }

    private void HandleValidSubmit()
    {
        Dispatcher.Dispatch(new AddRecipeClickedAction(RecipeContract, RecipeSource));
    }

    private void AddTag()
    {
        if (string.IsNullOrWhiteSpace(_currentTagEntry))
            return;

        if (RecipeContract.Tags.Contains(_currentTagEntry))
            return;

        RecipeContract.Tags.Add(_currentTagEntry);
    }
    private void RemoveTag(MudChip<string> chip) => RecipeContract.Tags.Remove(chip.Text!);

    private void AddIngredient() => RecipeContract.Ingredients.Add(new());
    private void RemoveIngredient(IngredientContract ingredient) => RecipeContract.Ingredients.Remove(ingredient);

    private void AddInstruction() => RecipeContract.Instructions.Add(new() { Order = RecipeContract.Instructions.Count + 1 });

    private void RemoveInstruction(InstructionContract instruction)
    {
        for (int i = instruction.Order; i < RecipeContract.Instructions.Count; i++)
            RecipeContract.Instructions[i].Order--;
        RecipeContract.Instructions.RemoveAt(instruction.Order - 1);
    }

    private void MoveInstructionUp(InstructionContract instruction)
    {
        if (instruction.Order == 1)
            return;

        var index = instruction.Order - 1;
        var temp = RecipeContract.Instructions[index - 1];
        RecipeContract.Instructions[index - 1] = RecipeContract.Instructions[index];
        RecipeContract.Instructions[index] = temp;

        RecipeContract.Instructions[index - 1].Order--;
        RecipeContract.Instructions[index].Order++;
    }

    private void MoveInstructionDown(InstructionContract instruction)
    {
        if (instruction.Order == RecipeContract.Instructions.Count)
            return;

        var index = instruction.Order - 1;
        var temp = RecipeContract.Instructions[index + 1];
        RecipeContract.Instructions[index + 1] = RecipeContract.Instructions[index];
        RecipeContract.Instructions[index] = temp;

        RecipeContract.Instructions[index + 1].Order++;
        RecipeContract.Instructions[index].Order--;
    }

    private void ClearForm()
    {
        RecipeContract = new RecipeContract { Servings = 4 };
        RecipeSource = RecipeState.Value.RecipieCollections.Select(x => x.Source).FirstOrDefault() ?? RecipeSource;
    }
}
