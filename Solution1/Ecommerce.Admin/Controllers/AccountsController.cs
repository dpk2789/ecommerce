using Ecommerce.Admin.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Admin.Controllers
{
    public class AccountsController : Controller
    {        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountsController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {           
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel viewModel)
        {
            var user = await _userManager.FindByNameAsync(viewModel.Username);
            if (user == null) return NotFound();
            var userRoles = await _userManager.FindByLoginAsync(viewModel.Username,viewModel.Password);
          
            return Ok();
        }
    }
}
