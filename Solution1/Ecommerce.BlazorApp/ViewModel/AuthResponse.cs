namespace Ecommerce.BlazorApp.ViewModel
{
    public class AuthResponse
    {
        public bool? Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? UserName { get; set; }
        public string? Token { get; set; }
        public int ExpriesIn { get; set; }
    }
}
