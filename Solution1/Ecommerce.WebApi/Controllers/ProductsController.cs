using Ecommerce.Infrastructure.IRepositories;
using Ecommerce.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public readonly IProductRepository _productRepository;
        public readonly ICartRepository _cartRepository;
        public ProductsController(IProductRepository repoWrapper, ICartRepository cartRepository)
        {
            _productRepository = repoWrapper;
            _cartRepository = cartRepository;
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var data = await _productRepository.FindAll().Include(x => x.ProductImages).Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Title = x.Title,
                    SalePrice = x.SalePrice,
                    Description = x.Description,
                    Size = x.Size,
                    Colour = x.Colour,
                    Brand = x.Brand,
                    MRP = x.MRP,
                    ProductImages = (List<ProductImageViewModel>)x.ProductImages.Select(y => new ProductImageViewModel
                    {
                        Id = y.Id,
                        Name = y.Name,
                        RelativePath = y.RelativePath,
                    })
                }).OrderBy(ow => ow.Name).ToListAsync();

                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetProductById(Guid Id)
        {
            var product = await _productRepository.FindByCondition(x => x.Id == Id).Include(x => x.ProductImages).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            var viewModel = new ProductViewModel();
            List<ProductImageViewModel> productImageList = new List<ProductImageViewModel>();
            var cartProduct = await _cartRepository.FindByCondition(x => x.ProductId == Id).FirstOrDefaultAsync();
            if (cartProduct == null)
            {
                viewModel.IsInCart = false;
            }
            else
            {
                viewModel.IsInCart = true;
            }
            viewModel.Id = product.Id;
            viewModel.ProductCategoryId = product.ProductCategoryId;
            viewModel.Title = product.Title;
            viewModel.SalePrice = product.SalePrice;
            viewModel.Description = product.Description;
            viewModel.Name = product.Name;
            viewModel.ModelNumber = product.ModelNumber;
            viewModel.Is_Taxable = product.Is_Taxable;
            viewModel.ProductTaxCode = product.ProductTaxCode;
            viewModel.Size = product.Size;
            viewModel.Colour = product.Colour;
            viewModel.MRP = product.MRP;
            viewModel.Brand = product.Brand;
            if (product.ProductImages != null)
            {
                foreach (var item in product.ProductImages)
                {
                    ProductImageViewModel productImageViewModel = new ProductImageViewModel();
                    productImageViewModel.Id = item.Id;
                    productImageViewModel.Name = item.Name;
                    productImageViewModel.RelativePath = item.RelativePath;
                    productImageList.Add(productImageViewModel);
                }
            }
            viewModel.ProductImages = productImageList;
            return Ok(viewModel);
        }
    }
}
