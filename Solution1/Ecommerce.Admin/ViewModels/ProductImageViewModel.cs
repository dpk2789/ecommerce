namespace Ecommerce.Admin.ViewModels
{
    public class ProductImageViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Type { get; set; }
        public string? Extention { get; set; }
        public long Length { get; set; }
        public string? RelativePath { get; set; }
        public string? GlobalPath { get; set; }
        public Guid ProductId { get; set; }       
    }
}
