using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace ComLib.MVC
{
    public class AjaxRedirectResult : ActionResult
    {
        private string _url;

        public AjaxRedirectResult(string url)
        {
            _url = url;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                var result = new JavaScriptResult()
                {
                    Script =
                            String.Format("window.location='{0}';",
                                          string.IsNullOrEmpty(_url)
                                              ? ""
                                              : UrlHelper.GenerateContentUrl(_url, context.HttpContext))
                };
                result.ExecuteResult(context);
            }
        }
    }

    public class AjaxRedirectToRouteResult : RedirectToRouteResult
    {
        private readonly string _action;
        private readonly string _controller;

        public AjaxRedirectToRouteResult(string action, string controller)
            : base(null, null)
        {
            _action = action;
            _controller = controller;
        }

        public AjaxRedirectToRouteResult(string routeName, string action, string controller)
            : base(routeName, null)
        {
            _action = action;
            _controller = controller;
        }



        public override void ExecuteResult(ControllerContext context)
        {
            if (context.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                var result = new JavaScriptResult()
                {
                    Script =
                            String.Format("window.location='{0}';",
                                          UrlHelper.GenerateUrl(RouteName, _action, _controller, null,
                                                                RouteTable.Routes,
                                                                context.RequestContext, true))
                };
                result.ExecuteResult(context);
            }
            else
                base.ExecuteResult(context);
        }
    }
}
