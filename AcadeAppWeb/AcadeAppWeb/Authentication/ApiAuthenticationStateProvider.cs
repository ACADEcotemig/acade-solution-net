using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.JSInterop;

namespace AcadeAppWeb.Authentication;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
 private readonly IJSRuntime _js;
 private const string TokenKey = "authToken";
 private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

 public ApiAuthenticationStateProvider(IJSRuntime js)
 {
 _js = js;
 }

 public override async Task<AuthenticationState> GetAuthenticationStateAsync()
 {
 var token = await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
 if (string.IsNullOrEmpty(token)) return new AuthenticationState(_anonymous);

 try
 {
 var handler = new JwtSecurityTokenHandler();
 var jwt = handler.ReadJwtToken(token);
 var identity = new ClaimsIdentity(jwt.Claims, "jwt");
 var user = new ClaimsPrincipal(identity);
 return new AuthenticationState(user);
 }
 catch
 {
 return new AuthenticationState(_anonymous);
 }
 }

 public void MarkUserAsAuthenticated(string token)
 {
 var handler = new JwtSecurityTokenHandler();
 var jwt = handler.ReadJwtToken(token);
 var identity = new ClaimsIdentity(jwt.Claims, "jwt");
 var user = new ClaimsPrincipal(identity);

 NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
 }

 public void MarkUserAsLoggedOut()
 {
 NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
 }
}
