namespace Ecommerce.WebApi.ViewModels
{
    public class AddToCartRequest
    {
        public Guid ProductId { get; set; }
        public int Qty { get; set; }
        public decimal? Price { get; set; }
        public string UserId { get; set; }
    }
}
