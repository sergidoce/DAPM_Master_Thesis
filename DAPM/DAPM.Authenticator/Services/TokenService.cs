using DAPM.Authenticator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DAPM.Authenticator.Services
{
    public class TokenService
    {
        private UserManager<User> _usermanager;
        private IConfiguration _config;
        private SymmetricSecurityKey _symmetricSecurityKey;

        public TokenService(IConfiguration configuration, UserManager<User> userManager) 
        {
            _usermanager = userManager;
            _config = configuration;
            _symmetricSecurityKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWTTokenKey").Value));

        }
        public async Task<string> CreateToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString())
            };

            IList<string> roles = await _usermanager.GetRolesAsync(user);

            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            var credentials =
                    new SigningCredentials(_symmetricSecurityKey,
                                          SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
