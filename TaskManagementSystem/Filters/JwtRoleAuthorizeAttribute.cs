using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TaskManagementSystem.Filters
{
    public class JwtRoleAuthorizeAttribute: AuthorizeAttribute
    {
        private readonly string[] allowedRoles;

        public JwtRoleAuthorizeAttribute(params string[] roles)
        {
            allowedRoles = roles;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var principal = actionContext.RequestContext.Principal;

            if (principal == null || !principal.Identity.IsAuthenticated)
                return false;

            var claimsPrincipal = principal as ClaimsPrincipal;

            if (claimsPrincipal == null)
                return false;

            var role = claimsPrincipal.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .FirstOrDefault();

            return allowedRoles.Contains(role);
        }


    }
}