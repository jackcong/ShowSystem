using System;
using System.Web.Mvc;

namespace ComLib.MVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class T2VAuthorize : AuthorizeAttribute
    {
        public string RequestType { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            OnAuthorizationHelp(filterContext);
        }

        internal void OnAuthorizationHelp(AuthorizationContext filterContext)
        {
            if (filterContext.Result is HttpUnauthorizedResult)
            {
                if (this.RequestType == "Ajax")
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.HttpContext.Response.End();
                }
            }
        }
    }
}
