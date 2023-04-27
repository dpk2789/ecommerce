namespace Ecommerce.WebApi.ViewModels
{
    public class ConfirmEmailRequest
    {
        public string? UserId { get; set; }
        public string? Code { get; set; }
    }
    public class ConfirmEmailResponse
    {
        public string? UserId { get; set; }
        public string? Code { get; set; }
        public string? StatusMessage { get; set; }
        public bool Success { get; set; }
    }
}
