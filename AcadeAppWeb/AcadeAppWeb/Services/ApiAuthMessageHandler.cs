using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace AcadeAppWeb.Services;

public class ApiAuthMessageHandler : DelegatingHandler
{
 private readonly IJSRuntime _js;
 private const string TokenKey = "authToken";

 public ApiAuthMessageHandler(IJSRuntime js)
 {
 _js = js;
 InnerHandler = new HttpClientHandler();
 }

 protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
 {
 var token = await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
 if (!string.IsNullOrEmpty(token))
 {
 request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
 }

 return await base.SendAsync(request, cancellationToken);
 }
}
