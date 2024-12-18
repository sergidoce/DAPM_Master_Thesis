using DAPM.Authenticator.Interfaces;
using DAPM.Authenticator.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UtilLibrary;

namespace DAPM.Authenticator.Services
{
    public partial class TokenService : ITokenService
    {
        private IUserManagerWrapper _usermanager;
        private IConfiguration _config;
        private SymmetricSecurityKey _symmetricSecurityKey;

        public TokenService(IConfiguration configuration, IUserManagerWrapper userManager) 
        {
            _usermanager = userManager;
            _config = configuration;
            _symmetricSecurityKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWTTokenKey").Value));

        }
        public async Task<string> CreateToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(CustomTokenTypeConstants.UserName, user.UserName),
                new Claim(CustomTokenTypeConstants.Id, user.Id.ToString()),
                new Claim(CustomTokenTypeConstants.OrganisationName, user.OrganizationName),
                new Claim(CustomTokenTypeConstants.OrganisationId, user.OrganizationId.ToString())
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
