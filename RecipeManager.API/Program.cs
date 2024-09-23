using Microsoft.EntityFrameworkCore;
using RecipeManager.API;
using RecipeManager.API.Services;
using RecipeManager.Shared.Db;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RecipeContext>(opt =>
{
    opt.UseSqlite($"Data Source={Settings.DbPath}");
});
builder.Services.AddScoped<IRecipesService, RecipesService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
