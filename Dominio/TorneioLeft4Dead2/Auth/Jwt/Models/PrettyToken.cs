using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace TorneioLeft4Dead2.Auth.Jwt.Models;

public class PrettyToken
{
    private long _expiresIn;

    public PrettyToken(JwtSecurityToken securityToken)
        : this()
    {
        AccessToken = securityToken.RawData;
        ExpiresIn = (long)(securityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
        Expires = securityToken.ValidTo;
        Claims = securityToken.Claims.ToArray();
    }

    public PrettyToken()
    {
        TokenType = "bearer";
    }

    public string TokenType { get; }
    public string AccessToken { get; }

    public long ExpiresIn
    {
        get => _expiresIn;
        set => _expiresIn = Math.Max(0, value);
    }

    public DateTime Expires { get; }
    public Claim[] Claims { get; }
}