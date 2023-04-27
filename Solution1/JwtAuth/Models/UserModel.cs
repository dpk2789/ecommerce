using System.ComponentModel.DataAnnotations;

namespace JwtAuth.Models
{
    public class UserModel
    {
        public string? Id { get; set; }
        public string? Username { get; set; }       
        public string? Password { get; set; }
    }
}
