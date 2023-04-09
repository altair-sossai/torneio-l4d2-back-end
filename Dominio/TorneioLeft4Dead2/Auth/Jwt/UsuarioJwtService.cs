using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using TorneioLeft4Dead2.Auth.Entities;
using TorneioLeft4Dead2.Auth.Jwt.Models;
using TorneioLeft4Dead2.Auth.Jwt.Services;

namespace TorneioLeft4Dead2.Auth.Jwt;

public static class UsuarioJwtService
{
    public static PrettyToken CreateNewToken(UserEntity user)
    {
        var identity = new GenericIdentity(user.Name);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Name),
            new Claim("userId", user.Id.ToString()),
            new Claim("name", user.Name),
            new Claim("email", user.Email)
        };
        var claimsIdentity = new ClaimsIdentity(identity, claims);

        var configuration = new UsuarioJwtConfiguration();
        var secretKeyService = new JwtSecretKeyService(configuration);
        var token = secretKeyService.CreatePrettyToken(claimsIdentity, DateTime.Now.AddYears(1));

        return token;
    }

    public static bool IsValidToken(string token)
    {
        var configuration = new UsuarioJwtConfiguration();
        var secretKeyService = new JwtSecretKeyService(configuration);

        return secretKeyService.IsValidToken(token);
    }

    public static ClaimsPrincipal ClaimsPrincipal(string token)
    {
        var configuration = new UsuarioJwtConfiguration();
        var secretKeyService = new JwtSecretKeyService(configuration);

        return secretKeyService.ClaimsPrincipal(token);
    }
}