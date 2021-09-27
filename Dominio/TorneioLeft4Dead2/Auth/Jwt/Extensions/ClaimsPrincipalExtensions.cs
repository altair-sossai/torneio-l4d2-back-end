using System;
using System.Security.Claims;

namespace TorneioLeft4Dead2.Auth.Jwt.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetGuid(this ClaimsPrincipal claimsPrincipal, string type)
        {
            var claim = claimsPrincipal.FindFirst(type);

            return claim == null ? Guid.Empty : Guid.Parse(claim.Value);
        }
    }
}