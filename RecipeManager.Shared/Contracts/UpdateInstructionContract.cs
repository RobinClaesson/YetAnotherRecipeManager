using RecipeManager.Shared.Models;

namespace RecipeManager.Shared.Contracts;

public record UpdateInstructionContract
{
    public Guid? InstructionId { get; set; }
    public string? Name { get; set; }
    public int? Order { get; set; }
    public string? Description { get; set; }
}
