using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TorneioLeft4Dead2.Auth.Jwt.Extensions;
using TorneioLeft4Dead2.Auth.Jwt.Models;

namespace TorneioLeft4Dead2.Auth.Jwt.Services;

public class JwtService
{
    protected JwtService(TokenValidationParameters validationParameters, JwtSecurityTokenHandler handler)
    {
        ValidationParameters = validationParameters;
        Handler = handler;
    }

    private TokenValidationParameters ValidationParameters { get; }
    private JwtSecurityTokenHandler Handler { get; }

    protected PrettyToken CreatePrettyToken(SecurityTokenDescriptor securityTokenDescriptor)
    {
        var token = CreateToken(securityTokenDescriptor);

        return Handler.PrettyToken(token);
    }

    public bool IsValidToken(string token)
    {
        return Handler.IsValidToken(token, ValidationParameters);
    }

    public ClaimsPrincipal ClaimsPrincipal(string token)
    {
        return Handler.ClaimsPrincipal(token, ValidationParameters);
    }

    private string CreateToken(SecurityTokenDescriptor securityTokenDescriptor)
    {
        return Handler.CreateToken(securityTokenDescriptor).Token(Handler);
    }
}