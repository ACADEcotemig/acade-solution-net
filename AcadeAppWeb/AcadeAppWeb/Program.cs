using AcadeAppWeb;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AcadeAppWeb.Services;
using AcadeAppWeb.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Use HTTP API base address for HttpClient
var apiBase = new Uri("http://localhost:5206");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = apiBase });

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();