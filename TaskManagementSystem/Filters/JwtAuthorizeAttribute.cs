using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TaskManagementSystem.Filters
{
    public class JwtAuthorizeAttribute: AuthorizeAttribute
    {

        protected override bool IsAuthorized(
            HttpActionContext actionContext)
        {
            try
            {
                string authHeader =
                    actionContext.Request.Headers.Authorization?.ToString();

                if (string.IsNullOrEmpty(authHeader))
                    return false;

                if (!authHeader.StartsWith("Bearer "))
                    return false;

                string token = authHeader.Substring(7);

                string key =
                    ConfigurationManager.AppSettings["JwtKey"];

                string issuer =
                    ConfigurationManager.AppSettings["JwtIssuer"];

                string audience =
                    ConfigurationManager.AppSettings["JwtAudience"];

                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(key)
                            ),

                        ValidateIssuer = true,
                        ValidIssuer = issuer,

                        ValidateAudience = true,
                        ValidAudience = audience,

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.Zero
                    };

                var principal =
                    tokenHandler.ValidateToken(
                        token,
                        validationParameters,
                        out SecurityToken validatedToken
                    );

                HttpContext.Current.User = principal;

                return true;
            }
            catch
            {
                return false;
            }
        }



    }
}