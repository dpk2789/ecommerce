namespace Ecommerce.WebApi.ViewModels
{
    public class UserRegisterResponse
    {
        public string? UserId { get; set; }
        public string? Code { get; set; }
        public string? CallbackUrl { get; set; }
        public List<string>? ErrorMessages { get; set; }
        public string? Msg { get; set; }
        public bool Success { get; set; }
    }
}
