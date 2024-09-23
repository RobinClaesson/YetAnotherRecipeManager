using CommandLine;
using RecipeManager.Shared;

namespace RecipeManager.API.CommandLine;

[Verb("run", isDefault: true, HelpText = "Run the RecipeManager API")]
public class RunOptions
{
    [Option('p', "http-port", Required = false, HelpText = $"The port to listen for HTTP requests.")]
    public int? HttpPort { get; set; } = null;
    public bool UseHttp => HttpPort != null;

    [Option('s', "https-port", Required = false, HelpText = "The port to listen for HTTPS requests. HTTPS turned of if not set.")]
    public int? HttpsPort { get; set; } = null;
    public bool UseHttps => HttpsPort != null;

    [Option('l', "localhost", Required = false, HelpText = "Allow connection only from localhost.")]
    public bool LocalHost { get; set; } = false;

    [Option('w', "swagger", Required = false, HelpText = "Allows direct API access with Swagger.")]
    public bool UseSwagger { get; set; }

    [Option('d', "db-path", Required = false, HelpText = $"The path to the SQLite database. Default: '{Defaults.DbPath}'")]
    public string DbPath { get; set; } = Defaults.DbPath;

    public string[] GetHostUrls()
    {
        var host = LocalHost ? "localhost" : "[::]";

        //HTTP is enabled, but HTTPS is not
        if (UseHttp && !UseHttps)
            return [$"http://{host}:{HttpPort}"];

        //HTTPS is enabled, but HTTP is not
        if (!UseHttp && UseHttps)
            return [$"https://{host}:{HttpsPort}"];

        //Both HTTP and HTTPS are enabled
        return [$"http://{host}:{HttpPort}", $"https://{host}:{HttpsPort}"];
    }
}