using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductApproval.Password_and_Authentication_Helpers
{
    public class JwtGenerator : ITokenGenerator
    {
        private readonly string secret;

        public JwtGenerator(string secret)
        {
            this.secret = secret;
        }

        public string GenerateToken(string username, string role)
        {
            var claims = new[]
{
                new Claim("sub", username),
                new Claim("rol", role),
                new Claim("iat", DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
