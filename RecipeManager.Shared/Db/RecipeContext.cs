using Microsoft.EntityFrameworkCore;
using RecipeManager.Shared.Models;

namespace RecipeManager.Shared.Db;

public class RecipeContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }

    public RecipeContext(DbContextOptions<RecipeContext> options) : base(options)
    {
        
    }
}
