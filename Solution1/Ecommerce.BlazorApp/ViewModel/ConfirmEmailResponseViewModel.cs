
namespace Ecommerce.BlazorApp.ViewModel
{
    public class ConfirmEmailResponseViewModel
    {
        public string? UserId { get; set; }
        public string? Code { get; set; }
        public string? StatusMessage { get; set; }
        public bool Success { get; set; }
    }
}
