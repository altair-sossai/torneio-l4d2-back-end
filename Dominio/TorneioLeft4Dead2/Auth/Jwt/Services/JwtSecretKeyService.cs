using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TorneioLeft4Dead2.Auth.Jwt.Configurations;
using TorneioLeft4Dead2.Auth.Jwt.Models;

namespace TorneioLeft4Dead2.Auth.Jwt.Services;

public class JwtSecretKeyService : JwtService
{
    public JwtSecretKeyService(SecretKeyJwtConfiguration configuration)
        : this(configuration.TokenValidationParameters, configuration.SecurityTokenHandler, configuration.SigningCredentials)
    {
    }

    private JwtSecretKeyService(TokenValidationParameters validationParameters,
        JwtSecurityTokenHandler handler,
        SigningCredentials signingCredentials)
        : base(validationParameters, handler)
    {
        SigningCredentials = signingCredentials;
    }

    private SigningCredentials SigningCredentials { get; }

    public PrettyToken CreatePrettyToken(ClaimsIdentity claimsIdentity, DateTime? expires = null)
    {
        var tokenDescriptor = BuildSecurityTokenDescriptor(claimsIdentity, expires);

        return CreatePrettyToken(tokenDescriptor);
    }

    private SecurityTokenDescriptor BuildSecurityTokenDescriptor(ClaimsIdentity claimsIdentity, DateTime? expires)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = expires ?? DateTime.Now.AddDays(1),
            SigningCredentials = SigningCredentials
        };

        return tokenDescriptor;
    }
}