
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Ecommerce.BlazorApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILocalStorageService _localStorage;        
        private readonly AuthenticationStateProvider _authStateProvider;
       
        public AuthService(AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
               
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<string> GetUserName()
        {
            if (await _localStorage.ContainKeyAsync("authToken"))
            {
                string? userName = String.Empty;
                var authState = await _authStateProvider.GetAuthenticationStateAsync();               
                 var user = authState.User;
                if(user.Identity != null)
                {
                    if (user.Identity.IsAuthenticated)
                    {
                        userName = user.FindFirst(c => c.Type == "sub")?.Value;
                    }
                }
               
                return userName;
            }
            return String.Empty;
        }

        public async Task<string> GetUserId()
        {
            if (await _localStorage.ContainKeyAsync("authToken"))
            {
                string? userName = String.Empty;
                var authState = await _authStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                if (user.Identity != null)
                {
                    if (user.Identity.IsAuthenticated)
                    {
                        userName = user.FindFirst(c => c.Type == "jti")?.Value;
                    }
                }

                return userName;
            }
            return String.Empty;
        }

    }
}
