using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TorneioLeft4Dead2.Auth.Jwt.Configurations;

public class SecretKeyJwtConfiguration
{
    private readonly string _secretKey;
    private JwtSecurityTokenHandler _securityTokenHandler;
    private SigningCredentials _signingCredentials;
    private TokenValidationParameters _tokenValidationParameters;

    protected SecretKeyJwtConfiguration(string secretKey)
    {
        _secretKey = secretKey;
    }

    public SigningCredentials SigningCredentials
    {
        get
        {
            if (_signingCredentials != null)
                return _signingCredentials;

            var signingKey = Encoding.UTF8.GetBytes(_secretKey);
            var securityKey = new SymmetricSecurityKey(signingKey);

            _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            return _signingCredentials;
        }
    }

    public TokenValidationParameters TokenValidationParameters
    {
        get
        {
            if (_tokenValidationParameters != null)
                return _tokenValidationParameters;

            _tokenValidationParameters = BuildTokenValidationParameters();

            return _tokenValidationParameters;
        }
    }

    public JwtSecurityTokenHandler SecurityTokenHandler => _securityTokenHandler ??= new JwtSecurityTokenHandler();

    private TokenValidationParameters BuildTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SigningCredentials.Key
        };
    }
}