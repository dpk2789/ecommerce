using System.ComponentModel.DataAnnotations;

namespace Ecommerce.WebApi.ViewModels
{
    public class ForgetPasswordRequest
    {
        [EmailAddress]
        public string? Email { get; set; }
    }
}
