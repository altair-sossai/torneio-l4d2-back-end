using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TorneioLeft4Dead2.Auth.Jwt.Models;

namespace TorneioLeft4Dead2.Auth.Jwt.Extensions
{
    public static class JwtSecurityTokenHandlerExtensions
    {
        public static PrettyToken PrettyToken(this JwtSecurityTokenHandler handler, string token)
        {
            var securityToken = handler.ReadJwtToken(token);

            return securityToken.PrettyToken();
        }

        public static bool IsValidToken(this JwtSecurityTokenHandler handler, string token, TokenValidationParameters validationParameters)
        {
            return ClaimsPrincipal(handler, token, validationParameters) != null;
        }

        public static ClaimsPrincipal ClaimsPrincipal(this JwtSecurityTokenHandler handler, string token, TokenValidationParameters validationParameters)
        {
            try
            {
                return handler.ValidateToken(token, validationParameters, out _);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}