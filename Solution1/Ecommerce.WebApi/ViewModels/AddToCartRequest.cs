namespace Ecommerce.WebApi.ViewModels
{
    public class AddToCartRequest
    {
        public Guid ProductId { get; set; }
        public int Qty { get; set; }
        public decimal? Value { get; set; }
        public string UserId { get; set; }
    }
}
