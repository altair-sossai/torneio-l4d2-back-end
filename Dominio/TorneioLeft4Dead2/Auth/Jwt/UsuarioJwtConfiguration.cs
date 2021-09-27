using System;
using TorneioLeft4Dead2.Auth.Jwt.Configurations;

namespace TorneioLeft4Dead2.Auth.Jwt
{
    public class UsuarioJwtConfiguration : SecretKeyJwtConfiguration
    {
        public UsuarioJwtConfiguration()
            : base(Environment.GetEnvironmentVariable("JwtSecretKey"))
        {
        }
    }
}