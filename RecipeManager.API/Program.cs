using CommandLine;
using Microsoft.EntityFrameworkCore;
using RecipeManager.API;
using RecipeManager.API.CommandLine;
using RecipeManager.API.Services;
using RecipeManager.Shared.Db;
using System.Text.Json.Serialization;

Parser.Default.ParseArguments<RunOptions>(args)
    .WithParsed(BuildAndRun)
    .WithNotParsed(HandleParseError);

void BuildAndRun(RunOptions options)
{
    if(!options.UseHttp && !options.UseHttps)
    {
        Console.WriteLine("Error: Either HTTP or HTTPS port must be set");
        return;
    }

    Settings.DbPath = options.DbPath;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllers()
                    .AddJsonOptions(opt =>
                    {
                        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDbContext<RecipeContext>(opt =>
    {
        opt.UseSqlite($"Data Source={Settings.DbPath}");
    });
    builder.Services.AddScoped<IRecipesService, RecipesService>();
    builder.Services.AddScoped<IIngredientsService, IngredientService>();
    builder.Services.AddScoped<ITagService, TagService>();

    // Add the ports from the command line options
    builder.WebHost.UseUrls(options.GetHostUrls());

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (options.UseSwagger)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        Console.WriteLine("Swagger UI enabled.");
    }

    if (options.UseHttps)
        app.UseHttpsRedirection();

    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}

void HandleParseError(IEnumerable<Error> errors)
{
    Console.WriteLine("Failed to parse command line arguments:");
    foreach (var error in errors)
    {
        Console.WriteLine(error);
    }
}
