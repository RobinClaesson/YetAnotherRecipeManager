﻿using System.ComponentModel.DataAnnotations;

namespace RecipeManager.Shared.Models;

public record Instruction
{
    [Key]
    public Guid InstructionId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    //Parent Recipe
    public Guid RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;
}
