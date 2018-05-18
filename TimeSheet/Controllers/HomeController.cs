using BizLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace TimeSheet.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //FormsAuthentication.SetAuthCookie(User.Identity.Name, true);

            //bool emailExists = AccountHelper.CheckEmailExists(User.Identity.Name);

            //if (emailExists)
            //{
            //    HttpContext.Response.Cookies.Add(new HttpCookie("Email", User.Identity.Name));
            //    HttpContext.Response.Cookies.Add(new HttpCookie("UserID", AccountHelper.GetUserID(User.Identity.Name).ToString()));
            //}
            //else
            //{
            //    int userID = AccountHelper.AddNewUser(User.Identity.Name);
            //    HttpContext.Response.Cookies.Add(new HttpCookie("Email", User.Identity.Name));
            //    HttpContext.Response.Cookies.Add(new HttpCookie("UserID", userID.ToString()));
            //}

            //return View();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}