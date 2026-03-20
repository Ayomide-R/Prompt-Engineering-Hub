using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace PromptHub.Web.Handlers;

public class JwtAuthorizationMessageHandler : DelegatingHandler
{
    private readonly IJSRuntime _jsRuntime;

    public JwtAuthorizationMessageHandler(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var token = await _jsRuntime.InvokeAsync<string>("eval", "localStorage.getItem('authToken')");

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        catch (Exception ex)
        {
            // Log error or ignore if JS interop is not yet available
            Console.WriteLine($"Token retrieval failed: {ex.Message}");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
