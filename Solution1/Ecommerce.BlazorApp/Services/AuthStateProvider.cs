

using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace Ecommerce.BlazorApp.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _anonymous;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (!await _localStorage.ContainKeyAsync("authToken"))
                {
                    return _anonymous;
                }
                else
                {
                    var token = await _localStorage.GetItemAsync<string>("authToken");

                    if (string.IsNullOrWhiteSpace(token))
                        return _anonymous;
                    var claims = ParseClaimsFromJwt(token);
                    var expiry = claims.Where(claim => claim.Type.Equals("exp")).FirstOrDefault();
                    if (expiry == null)
                        return _anonymous;
                    var datetime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiry.Value));
                    var cookieCmp = await _localStorage.GetItemAsync<string>("cmpCookee");
                    var cmpNameCookee = await _localStorage.GetItemAsync<string>("cmpNameCookee");
                    if (datetime.UtcDateTime <= DateTime.UtcNow)
                    {
                        await _localStorage.RemoveItemAsync("authToken");
                        if (!string.IsNullOrEmpty(cookieCmp))
                        {
                            await _localStorage.RemoveItemAsync("cmpCookee");
                            await _localStorage.RemoveItemAsync("cmpNameCookee");
                        }
                        return _anonymous;
                    }
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return _anonymous;

        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
        public void NotifyUserAuthentication(string email)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }
        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
