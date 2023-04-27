using System.ComponentModel.DataAnnotations;

namespace Ecommerce.BlazorApp.ViewModel
{
    public class LoginViewModel
    {       
        
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
