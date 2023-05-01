
using Ecommerce.Infrastructure.Domains;
using Ecommerce.Infrastructure.IRepositories;
using Ecommerce.WebApi.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        public readonly ICartRepository _cartRepository;
        public CartController(UserManager<IdentityUser> userManager, ICartRepository cartRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
        }

        [HttpGet("GetCartProducts")]
        public async Task<IActionResult> CartProducts(string userName)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userName);
                if (user == null) return NotFound();
                var data = await _cartRepository.FindByCondition(x => x.UserId == user.Id).Include(x => x.Product).Select(x => new CartProductViewModel
                {
                    Id = x.Id,
                    Name = x.Product.Name,
                    Title = x.Product.Title,
                    SalePrice = x.Product.SalePrice,
                    Qty = x.Qty,
                    Value = x.Value,
                    Size = x.Product.Size,
                    Colour = x.Product.Colour,
                    Brand = x.Product.Brand,
                    MRP = x.Product.MRP,
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

        [HttpPost("addtocart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.UserId);
                if (user == null) return NotFound();

                request.UserId = user.Id;
                var cartProduct = await _cartRepository.FindByCondition(x => x.ProductId == request.ProductId && x.UserId == user.Id).FirstOrDefaultAsync();

                if (cartProduct != null)
                {
                    cartProduct.Qty += request.Qty;
                    _cartRepository.Update(cartProduct);
                }
                else
                {
                    var newcartProduct = new CartProduct()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = request.ProductId,
                        Qty = request.Qty,
                        Value = request.Price.Value * request.Qty,
                        UserId = request.UserId
                    };
                    _cartRepository.Create(newcartProduct);
                }
                await _cartRepository.SaveAsync();
                return Ok("Item Added to cart");
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to add to cart");
            }
        }
    }
}
