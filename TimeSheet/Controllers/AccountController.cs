using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using WebModel.Account;
using System.Web.Security;
using BizLogic;

namespace TimeSheet.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult ShowLogOn()
        {
            return View("~/Views/Account/LogOn.cshtml");
        }


        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            Session.RemoveAll();
            AccountHelper accountHelper = new AccountHelper();
            ViewBag.ReturnUrl = string.IsNullOrEmpty(returnUrl) ? (Request["returnUrl"] ?? "") : returnUrl;
            if (ModelState.IsValid)
            {
                int returnResut = accountHelper.UserLogon(model.UserName, model.Password);
                if (returnResut == 1)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    Session["ReleaseNoteFlag"] = false;
                    //get user 
                    UserModel user = accountHelper.GetUserByName(model.UserName);
                    HttpContext.Response.Cookies.Add(new HttpCookie("UserName", user.DisplayName));
                    HttpContext.Response.Cookies.Add(new HttpCookie("UserID", user.ID.ToString()));
                    HttpContext.Response.Cookies.Add(new HttpCookie("UserEmail", user.Email));
                    HttpContext.Response.Cookies.Add(new HttpCookie("UserGroup", user.UserGroupID));

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(ViewBag.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (returnResut == 2)
                {
                    ModelState.AddModelError("", "User account is inactive, please contact your Administrator.");
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Invalid UserName or Password.");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public void SignIn()
        {
            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void SignOut()
        {
            string callbackUrl = Url.Action("SignOutCallback", "Account", routeValues: null, protocol: Request.Url.Scheme);

            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public void SignOutCallback()
        {
            if (Request.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                //return RedirectToAction("Index", "Home");
            }

            //return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public JsonResult SendChangePassword(string oldPassword, string newPassword)
        {
            AccountHelper accountHelper = new AccountHelper();
            if (accountHelper.ValidateOldPassword(oldPassword))
            {
                //update password.
                accountHelper.UpdatePassword(newPassword);

                return Json(new { result = 1 });
            }
            else
            {
                return Json(new { result = 0 });
            }
        }
    }
}
