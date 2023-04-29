using Ecommerce.Infrastructure.IRepositories;
using Ecommerce.WebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public readonly IProductRepository _productRepository;
        public ProductsController(IProductRepository repoWrapper)
        {
            _productRepository = repoWrapper;
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var data = await _productRepository.FindAll().Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Title = x.Title,
                    SalePrice = x.SalePrice,
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
            var product = await _productRepository.FindByCondition(x => x.Id == Id).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            var viewModel = new ProductViewModel();
            viewModel.Id = product.Id;
            viewModel.ProductCategoryId = product.ProductCategoryId;
            viewModel.Title= product.Title;
            viewModel.SalePrice = product.SalePrice;
            viewModel.Description= product.Description;
            viewModel.Name = product.Name;
            viewModel.ModelNumber = product.ModelNumber;
            viewModel.Is_Taxable = product.Is_Taxable;
            viewModel.ProductTaxCode = product.ProductTaxCode;
            return Ok(viewModel);
        }
    }
}
