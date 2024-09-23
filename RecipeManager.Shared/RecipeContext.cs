using Microsoft.EntityFrameworkCore;
using RecipeManager.Shared.Models;

namespace RecipeManager.Shared;

public class RecipeContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }

    public string DbPath { get; private set; } = "RecipeManager.sqlite";

    public RecipeContext()
    {
        
    }

    public RecipeContext(string dbPath)
    {
        DbPath = dbPath;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}
