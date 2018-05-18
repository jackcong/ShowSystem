using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace ComLib.MVC
{
    [T2VAuthorize]
    public class T2VController : Controller
    {
        // TODO: Much more func to extend.
        private readonly object _userLock = new object();

        protected T2VController()
        {

        }
        protected string RenderPartialToString(string viewName)
        {
            var pvr = PartialView(viewName);
            pvr.View = new RazorView(ControllerContext, viewName, null, true, new[] { "cshtml", "vbhtml", "ascx" });

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter tw = new HtmlTextWriter(sw))
                {
                    pvr.View.Render(new ViewContext(ControllerContext, pvr.View, (ViewDataDictionary)ViewData,(TempDataDictionary)TempData, tw), tw);
                }
            }

            return sb.ToString();
        }
        protected JsonNetResult JsonNet(object obj)
        {
            return new JsonNetResult { Result = obj };
        }


        protected AjaxRedirectResult AjaxRedirect(string url)
        {
            return new AjaxRedirectResult(url);
        }

        protected AjaxRedirectToRouteResult AjaxRedirectToAction(string action, string controller)
        {
            return new AjaxRedirectToRouteResult(action, controller);
        }
        public string ToStringN(int? intTar)
        {
            string str = intTar.ToString();
            int intTemp = int.Parse(str);
            return intTemp.ToString("N0");
        }
        public string ToStringN2(decimal? deTar)
        {
            string str = deTar.ToString();
            decimal deTemp = Decimal.Parse(str);
            return deTemp.ToString("N2");
        }
        public string ToStringT(DateTime? dtTar)
        {
            string str = dtTar.ToString();
            DateTime dtTemp = DateTime.Parse(str);
            return dtTemp.ToString("yyyy-MM-dd");
        }
    }
}