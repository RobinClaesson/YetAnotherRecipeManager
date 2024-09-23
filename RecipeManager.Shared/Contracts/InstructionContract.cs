using RecipeManager.Shared.Models;

namespace RecipeManager.Shared.Contracts;

public record InstructionContract
{
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    
    public override string ToString()
    {
        return $"{Order}. {Name}: {Description}";
    }

    public Instruction ToModel()
    {
        return new Instruction
        {
            Name = Name,
            Order = Order,
            Description = Description
        };
    }
}
