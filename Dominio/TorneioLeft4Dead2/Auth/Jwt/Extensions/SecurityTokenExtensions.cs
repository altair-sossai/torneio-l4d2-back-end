using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace TorneioLeft4Dead2.Auth.Jwt.Extensions
{
    public static class SecurityTokenExtensions
    {
        public static string Token(this SecurityToken securityToken, JwtSecurityTokenHandler handler)
        {
            return handler.WriteToken(securityToken);
        }
    }
}