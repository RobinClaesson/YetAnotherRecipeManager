﻿using RecipeManager.Shared.Models;

namespace RecipeManager.Shared.Contracts;

public record UpdateInstructionContract
{
    public Guid? InstructionId { get; set; }
    public string? Name { get; set; }
    public int? Order { get; set; }
    public string? Description { get; set; }

    public static UpdateInstructionContract FromModel(Instruction instruction)
        => new UpdateInstructionContract
        {
            InstructionId = instruction.InstructionId,
            Name = instruction.Name,
            Order = instruction.Order,
            Description = instruction.Description
        };

    public Instruction ToModel(Guid recipeId)
        => new Instruction
        {
            InstructionId = InstructionId ?? default,
            Name = Name ?? string.Empty,
            Order = Order ?? 0,
            Description = Description ?? string.Empty,
            RecipeId = recipeId
        };
}
