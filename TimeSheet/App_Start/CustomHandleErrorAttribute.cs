using ComLib.Extension;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeSheet.App_Start
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];
            

            ExceptionAttribute exceptionAttr = (ExceptionAttribute)Attribute.GetCustomAttribute(filterContext.Exception.GetType(),typeof(ExceptionAttribute));

            if (exceptionAttr._operationType == "RollBackAndSendEmail")
            {
                //TBD
            }
            if (exceptionAttr._operationType == "SendEmailAndLogInSystem")
            {
                //Do log and send email.
            }
            filterContext.Result = new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    error = true,
                    message = filterContext.Exception.Message,
                    
                }
            };

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}