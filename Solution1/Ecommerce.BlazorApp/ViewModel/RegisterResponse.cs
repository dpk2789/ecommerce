

namespace Ecommerce.BlazorApp.ViewModel
{
    public class RegisterResponse
    {
        public string? UserId { get; set; }
        public string? Msg { get; set; }
        public string? ErrorMessage { get; set; }
        public bool Success { get; set; }
        public string? Code { get; set; }
        public string? CallbackUrl { get; set; }
    }
}
