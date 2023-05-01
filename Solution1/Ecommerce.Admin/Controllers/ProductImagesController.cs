using Ecommerce.Admin.ViewModels;
using Ecommerce.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Admin.Controllers
{
    public class ProductImagesController : Controller
    {
        public readonly IProductRepository _productRepository;
        public readonly IProductImageRepository _productImageRepository;
        public ProductImagesController(IProductRepository productRepository, IProductImageRepository productImageRepository)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
        }
        public async Task<ActionResult<IList<ProductImageViewModel>>> Index(Guid productId)
        {
            var data = await _productImageRepository.FindByCondition(x => x.ProductId == productId).Select(x => new ProductImageViewModel
            {
                Id = x.Id,
                Name = x.Name,
            }).OrderBy(ow => ow.Name).ToListAsync();

            return View(data);
        }
    }
}
