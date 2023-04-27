using Ecommerce.WebApi.ViewModels;
using JwtAuth;
using JwtAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Web;

namespace Ecommerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly JwtTokenHandler _jwtTokenHandler;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountsController(JwtTokenHandler jwtTokenHandler, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _jwtTokenHandler = jwtTokenHandler;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel authRequest)
        {
            var user = await _userManager.FindByNameAsync(authRequest.Username);
            if (user == null) return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);
            UserModel userModel = new();
            userModel.Id = user.Id;
            userModel.Username = user.UserName;
            userModel.Password = authRequest.Password;
            var authResponse = _jwtTokenHandler.GenerateJwtToken(userModel, userRoles);
            return Ok(authResponse);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            return Ok(new UserRegisterResponse
            {
                UserId = user.Id,
                Code = code,
                Success = true,
                Msg = "User created successfully!"
            });
        }

        [HttpPost("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            if (request.UserId == null || request.Code == null)
            {
                return BadRequest("/");
            }
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound();
            }

            request.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            var result = await _userManager.ConfirmEmailAsync(user, request.Code);
            if (!result.Succeeded)
            {
                return Ok(new ConfirmEmailResponse
                {
                    UserId = user.Id,
                    Code = request.Code,
                    Success = false,
                    StatusMessage = "Error confirming your email"
                });
            }

            return Ok(new ConfirmEmailResponse
            {
                UserId = user.Id,
                Code = request.Code,
                Success = true,
                StatusMessage = "Thank you for confirming your email."
            });
        }

        [HttpPost("ForgetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser == null)
            {
                return Ok(new ForgetPasswordResponse
                {
                    ErrorMessages = { "User is not registered" },
                    Success = false
                });
            };

            var user = new IdentityUser { UserName = request.Email, Email = request.Email };

            var code = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
            // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var newCode = HttpUtility.UrlEncode(code);

            return Ok(new ForgetPasswordResponse
            {
                UserId = user.Id,
                Code = newCode,
                Success = true,
                IsEmailConfirmed = existingUser.EmailConfirmed
            });
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser == null)
            {
                return Ok(new ResetPasswordResponse
                {
                    ErrorMessages = { "User is not registered" },
                    Success = false
                });
            };

            var user = new IdentityUser { UserName = request.Email, Email = request.Email };
            // var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            var code = HttpUtility.UrlDecode(request.Code);
            var result = await _userManager.ResetPasswordAsync(existingUser, code, request.Password);
            if (result.Succeeded)
            {
                return Ok(new ResetPasswordResponse
                {
                    Success = true,
                });
            }
            else
            {
                List<string> _ErrorMessages = null;
                foreach (var error in result.Errors)
                {
                    _ErrorMessages.Add(error.Description);
                }
                return Ok(new ResetPasswordResponse { ErrorMessages = _ErrorMessages, Success = false });
            };
        }

        [HttpGet("getuser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
