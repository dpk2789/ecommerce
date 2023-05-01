namespace Ecommerce.WebApi.ViewModels
{
    public class CartProductViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public string? Code { get; set; }
        public string? ModelNumber { get; set; }
        public string? Title { get; set; }
        public int Qty { get; set; }
        public decimal Value { get; set; }
        public decimal? SalePrice { get; set; }
        public string? Size { get; set; }
        public string? Colour { get; set; }
        public string? Brand { get; set; }
        public string? MRP { get; set; }
    }
}
