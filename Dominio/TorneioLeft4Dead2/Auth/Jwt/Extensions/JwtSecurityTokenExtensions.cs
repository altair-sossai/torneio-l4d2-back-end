using System.IdentityModel.Tokens.Jwt;
using TorneioLeft4Dead2.Auth.Jwt.Models;

namespace TorneioLeft4Dead2.Auth.Jwt.Extensions;

public static class JwtSecurityTokenExtensions
{
    public static PrettyToken PrettyToken(this JwtSecurityToken securityToken)
    {
        return new PrettyToken(securityToken);
    }
}