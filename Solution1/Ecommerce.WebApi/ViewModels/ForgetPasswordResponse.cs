namespace Ecommerce.WebApi.ViewModels
{
    public class ForgetPasswordResponse
    {
        public bool? IsEmailConfirmed { get; set; }
        public string? UserId { get; set; }
        public string? Code { get; set; }
        public List<string>? ErrorMessages { get; set; }
        public bool Success { get; set; }
    }
}
