using BizLogic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebModel.Account;

namespace TimeSheet.Controllers
{
    public class SettingController : Controller
    {
        UserModel user;
        TimeSheetBizLogic tsb;

        public SettingController()
        {
            user = AccountHelper.GetCurrentUser();
            tsb = new TimeSheetBizLogic(user);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CategoryEdit()
        {
            return View();
        }

        public JsonResult GetFullCategory()
        {
            var category = tsb.getFullCategory();
            return Json(new {category = JsonConvert.SerializeObject(category) });
        }
        public JsonResult SaveNewNode(int Id,string level,string nodeName)
        {
            if (tsb.checkNodeNameExists(level, nodeName) == true)
            {
                return Json(new { success=false,content="Node name exists."});
            }

            int returnId = tsb.saveNewNode(Id, level, nodeName);
            return Json(new { success = true,Id= returnId });
        }

        public JsonResult SaveNode(int Id, string level, string nodeName)
        {
            int returnResult = tsb.saveNode(Id, level, nodeName);
            return Json(new { success = true, Id = returnResult });
        }

        public JsonResult DeleteNode(int Id, string level)
        {
            int returnResult = tsb.deleteNode(Id, level);
            return Json(new { success = true, Id = returnResult });
        }

    }
}