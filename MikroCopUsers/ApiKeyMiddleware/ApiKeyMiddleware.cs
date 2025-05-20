public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string API_KEY_HEADER_NAME = "X-API-KEY";

    // Replace with your actual API key store (e.g., database,config or whatever )
    private static readonly Dictionary<string, string> ValidApiKeys = new()
    {
        { "12345", "ClientA" },
        { "67890", "ClientB" }
    };

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey) ||
            !ValidApiKeys.ContainsKey(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is missing or invalid.");
            return;
        }

        // Store the client name for logging later
        context.Items["ClientName"] = ValidApiKeys[extractedApiKey];
        await _next(context);
    }
}