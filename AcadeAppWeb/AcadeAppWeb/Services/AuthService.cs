using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Authorization;
using AcadeAppWeb.Authentication;

namespace AcadeAppWeb.Services;

public class AuthService
{
 private readonly HttpClient _http;
 private readonly IJSRuntime _js;
 private readonly AuthenticationStateProvider _authStateProvider;
 private const string TokenKey = "authToken";

 public AuthService(HttpClient http, IJSRuntime js, AuthenticationStateProvider authStateProvider)
 {
 _http = http;
 _js = js;
 _authStateProvider = authStateProvider;
 }

 public async Task<bool> LoginAsync(string email, string senha)
 {
 var resp = await _http.PostAsJsonAsync("/api/auth/login", new { Email = email, Senha = senha });
 if (!resp.IsSuccessStatusCode) return false;

 using var doc = await JsonDocument.ParseAsync(await resp.Content.ReadAsStreamAsync());
 if (!doc.RootElement.TryGetProperty("token", out var tokenEl)) return false;

 var token = tokenEl.GetString();
 if (string.IsNullOrEmpty(token)) return false;

 await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);

 if (_authStateProvider is ApiAuthenticationStateProvider provider)
 provider.MarkUserAsAuthenticated(token);

 return true;
 }

 // New: authenticate and return minimal user info, with diagnostic support
 public async Task<UserInfo?> AuthenticateAsync(string email, string senha)
 {
 var resp = await _http.PostAsJsonAsync("/api/auth/login", new { Email = email, Senha = senha });
 if (!resp.IsSuccessStatusCode)
 {
 // optionally read error body for debugging
 try
 {
 var text = await resp.Content.ReadAsStringAsync();
 }
 catch { }
 return null;
 }

 using var doc = await JsonDocument.ParseAsync(await resp.Content.ReadAsStreamAsync());
 if (!doc.RootElement.TryGetProperty("token", out var tokenEl)) return null;
 var token = tokenEl.GetString();
 if (string.IsNullOrEmpty(token)) return null;

 // store token
 await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
 if (_authStateProvider is ApiAuthenticationStateProvider provider)
 provider.MarkUserAsAuthenticated(token);

 // parse user
 if (doc.RootElement.TryGetProperty("user", out var userEl))
 {
 try
 {
 var user = JsonSerializer.Deserialize<UserInfo>(userEl.GetRawText());
 return user;
 }
 catch
 {
 return null;
 }
 }

 return null;
 }

 public async Task LogoutAsync()
 {
 await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
 if (_authStateProvider is ApiAuthenticationStateProvider provider)
 provider.MarkUserAsLoggedOut();
 }

 public async Task<string?> GetTokenAsync()
 {
 return await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
 }

 public class UserInfo
 {
 public int Id { get; set; }
 public string Nome { get; set; } = string.Empty;
 public string Email { get; set; } = string.Empty;
 }
}
