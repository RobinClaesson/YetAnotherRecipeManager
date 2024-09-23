namespace RecipeManager.CLI;

internal static class HttpClientFactory
{
    public static HttpClient GetClient(BaseOptions options)
    {
        var client = new HttpClient();
        client.BaseAddress = new UriBuilder
        {
            Scheme = options.UseHttps ? "https" : "http",
            Host = options.Host,
            Port = options.Port
        }.Uri;
        return client;
    }
}
