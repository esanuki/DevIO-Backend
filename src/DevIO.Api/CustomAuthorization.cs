using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevIO.Api
{
    public class CustomAuthorization
    {
        public static bool ValidarUsuario(HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity.IsAuthenticated &&
                context.User.Claims.Any(c => c.Type.Equals(claimName) && c.Value.Contains(claimValue));
        }

        public class RequisitoClaimFilter : IAuthorizationFilter
        {
            private readonly Claim _claim;

            public RequisitoClaimFilter(Claim claim)
            {
                _claim = claim;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (!context.HttpContext.User.Identity.IsAuthenticated)
                {
                    context.Result = new StatusCodeResult(401);
                    return;
                }

                if (!CustomAuthorization.ValidarUsuario(context.HttpContext, _claim.Type, _claim.Value)) context.Result = new StatusCodeResult(403);
            }
        }

        public class ClaimsAuthorizeAttribute : TypeFilterAttribute
        {
            public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
            {
                Arguments = new object[] { new Claim(claimName, claimValue) };
            }
        }

    }
}
