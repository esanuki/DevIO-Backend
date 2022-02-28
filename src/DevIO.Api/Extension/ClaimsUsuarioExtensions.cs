using System;
using System.Security.Claims;

namespace DevIO.Api.Extension
{
    public static class ClaimsUsuarioExtensions
    {
        public static string ObterUsuarioId(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentException(nameof(principal));

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string ObterUsuarioEmail(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentException(nameof(principal));

            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
