
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

        [HttpPost("addtocart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
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
                        ProductId = request.ProductId,
                        Qty = request.Qty,
                        Value = request.Value.Value,
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
