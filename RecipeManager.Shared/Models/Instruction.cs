using System.ComponentModel.DataAnnotations;

namespace RecipeManager.Shared.Models;

public record Instruction
{
    [Key]
    public Guid InstructionId { get; set; }

    public string Name { get; set; } = string.Empty;
    public int Order { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    
    //Parent Recipe
    public Guid RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;

    public override string ToString()
    {
        return $"{Order}. {Name}: {Description}";
    }
}
