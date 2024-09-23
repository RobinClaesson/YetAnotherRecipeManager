﻿using Microsoft.EntityFrameworkCore;
using RecipeManager.Shared.Models;

namespace RecipeManager.Shared.Db;

public class RecipeContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }

    public string DbPath { get; private set; } = Defaults.DbPath;

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
