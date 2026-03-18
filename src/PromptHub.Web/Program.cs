using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using PromptHub.Web;
using PromptHub.Web.Providers;
using PromptHub.Web.Services;
using PromptHub.Web.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register MudBlazor
builder.Services.AddMudServices();

// Register the delegating handler
builder.Services.AddTransient<JwtAuthorizationMessageHandler>();

// Register the HttpClient with the handler
builder.Services.AddHttpClient("PromptHubApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/"); // Replace with actual API URL
})
.AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

// Register default HttpClient for generic use (e.g., Auth endpoints that don't need the token yet)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });

// Register Auth Services
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PromptService>();

await builder.Build().RunAsync();
