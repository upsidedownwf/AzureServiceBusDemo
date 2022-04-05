using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AzureServiceBusDemoWebAPI.Models.Attributes
{
    public static class Roles
    {
        public const string DIRECTOR = "DIRECTOR";
        public const string SUPERVISOR = "SUPERVISOR";
        public const string ANALYST = "ANALYST";
    }
    public static class AjaxExtension
    {
        //HttpRequest Extension method to 
        //check if the incoming request is an AJAX call - JRozario 
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
    }
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(params string[] claim) : base(typeof(AuthorizeFilter))
        {
            Arguments = new object[] { claim };
        }
    }

    public class AuthorizeFilter : IAuthorizationFilter
    {
        readonly string[] _claim;

        public AuthorizeFilter(params string[] claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var IsAuthenticated =
                   context.HttpContext.User.Identity.IsAuthenticated;
            var claimsIndentity =
                   context.HttpContext.User.Identity as ClaimsIdentity;

            if (IsAuthenticated)
            {
                bool flagClaim = false;
                foreach (var item in _claim)
                {
                    if (context.HttpContext.User.HasClaim("ACCESS_LEVEL", item))
                        flagClaim = true;
                }
                if (!flagClaim)
                {
                    if (context.HttpContext.Request.IsAjaxRequest())
                        context.HttpContext.Response.StatusCode =
                        (int)HttpStatusCode.Unauthorized; //Set HTTP 401 
                                                          //Unauthorized - JRozario
                    else context.Result =
                        new ObjectResult("Unauthorized") { StatusCode = (int)HttpStatusCode.Unauthorized };
                        //context.Result =
                        //     new RedirectResult("~/Dashboard/NoPermission");
                }
            }
            else
            {
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    context.Result =
                           new ObjectResult("Forbidden") { StatusCode = StatusCodes.Status403Forbidden };
                }
                else
                {
                    context.Result =
                           new ObjectResult("Forbidden") { StatusCode = StatusCodes.Status403Forbidden };
                    //context.Result = new RedirectResult("~/Home/Index");
                }
            }
            return;
        }
    }
    public static class PermissionExtension
    {
        public static bool HavePermission(this Controller c, string claimValue)
        {
            var user = c.HttpContext.User as ClaimsPrincipal;
            bool havePer = user.HasClaim(claimValue, claimValue);
            return havePer;
        }
        public static bool HavePermission(this IIdentity claims, string claimValue)
        {
            var userClaims = claims as ClaimsIdentity;
            bool havePer = userClaims.HasClaim(claimValue, claimValue);
            return havePer;
        }
    }
    public class AuthenticateAttribute : TypeFilterAttribute
    {
        public AuthenticateAttribute() : base(typeof(AuthenticateFilter))
        {
            //Empty constructor
        }
    }
    public class AuthenticateFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
            if (!IsAuthenticated)
            {
                if (context.HttpContext.Request.IsAjaxRequest())
                {

                    context.Result =
                           new ObjectResult("Forbidden") { StatusCode = StatusCodes.Status403Forbidden }; //Set HTTP 403 Forbidden - JRozario
                }
                else
                {
                    context.Result =
                           new ObjectResult("Forbidden") { StatusCode = StatusCodes.Status403Forbidden };
                    //context.Result = new RedirectResult("~/Home/Index");
                }
            }
        }
    }
}
