namespace Ecommerce.BlazorApp.Services
{
    public interface IAuthService
    {
        Task<string> GetUserName();
        Task<string> GetUserId();
    }
}