using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace TaskManagementSystem.Filters
{
    public class RoleAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        private string[] roles;

        public RoleAuthorizeAttribute(params string[] roles)
        {
            this.roles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["UserId"] == null)
                return false;

            string userRole = Convert.ToString(
                httpContext.Session["Role"]
            );

            foreach (string role in roles)
            {
                if (userRole == role)
                    return true;
            }

            return false;
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary
                {
            { "controller", "Home" },
            { "action", "AccessDenied" }
                });
        }



    }
}