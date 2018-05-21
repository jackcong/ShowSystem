using BizLogic;
using ComLib.HTTPResultHelpers;
using ComLib.MVC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebModel.Account;
using WebModel.Col;
using WebModel.TimeSheet;

namespace WebV2.Controllers
{
    [T2VAuthorize]
    public class DashBoardController : T2VController
    {
        UserModel user;
        TimeSheetBizLogic tsb;
        public DashBoardController()
        {
            user = AccountHelper.GetCurrentUser();
            tsb = new TimeSheetBizLogic(user);
        }

        public ActionResult Index(int page,string search)
        {
            return View();
        }

     

        public JsonResult GetEditInfo()
        {
            var res = tsb.GetEditInfo();
           
            return Json(new { showsys = JsonConvert.SerializeObject(res) });
        }

    }
}