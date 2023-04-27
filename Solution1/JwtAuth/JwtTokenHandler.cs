using JwtAuth.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuth
{
    public class JwtTokenHandler
    {
        public const string secretKey = "dd1762da-eb43-4f0d-b7f8-9a241ab11b87";
        public const int expiryTime = 360;

        public AuthResponse GenerateJwtToken(UserModel user, IList<string> roles)
        {
            var expiry = DateTime.UtcNow.AddMinutes(expiryTime);
            var key = Encoding.ASCII.GetBytes(secretKey);
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(key);
            SigningCredentials credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            //add claims
            var claims = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                 new Claim(JwtRegisteredClaimNames.Jti, user.Id),
            });

         
            foreach (var userRole in roles)
            {
                new Claim(ClaimTypes.Role, userRole);
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = expiry,
                SigningCredentials = credentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            var tokenString = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthResponse
            {
                UserName = user.Username,
                Token = tokenString,
                ExpriesIn = (int)expiry.Subtract(DateTime.Now).TotalSeconds
            };
        }
    }

}
