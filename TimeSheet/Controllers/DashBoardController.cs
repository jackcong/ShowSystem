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

        public ActionResult Detail(int DetailID)
        {
            return View();
        }

        public JsonResult GetDetailData(int DetailID)
        {
            try
            {
                var summary = tsb.GetSummaryAndTimeSheet(DetailID);
                var category = tsb.getFullCategory();
                return Json(new { summary = JsonConvert.SerializeObject(summary), category = JsonConvert.SerializeObject(category) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult CheckSummaryExists(int year,int week,int detailid)
        {
            int iExists = tsb.CheckSummaryExists(year,week,detailid);
            return Json(new { result = iExists });
        }

        public JsonResult GetCommentData(int timesheetid)
        {
            string strComment = tsb.GetTimeSheetComment(timesheetid);
            return Json(new { result = strComment });
        }

        public ActionResult Statistics()
        {
            return View();
        }

        public ActionResult Comparison()
        {
            return View();
        }

        public JsonResult GetCategory()
        {
            return Json(new { result = JsonConvert.SerializeObject(tsb.GetCategory()) });
        }

        public JsonResult SaveTimeSheet(SummaryTimeSheetModel stsm,TimeSheetModel tsm)
        {
            int success = tsb.SaveTimeSheet(stsm,tsm);
            return Json(new { result = success });
        }

        public ActionResult GetList(int currentPageIndex, int pageSize, string sortName, string sortOrder, string SearchParam)
        {
            //Get a claim biz logic instance.
            string fullsearch = string.Empty;
            if (!string.IsNullOrEmpty(SearchParam))
            {
                var listCol = JsonConvert.DeserializeObject(SearchParam, typeof(List<colModel>)) as List<colModel>;
                foreach (colModel colmodel in listCol)
                {
                    if (colmodel.columnName == "fullsearch")
                    {
                        fullsearch = colmodel.columnValue;
                        continue;
                    }
                }
            }
            fullsearch = fullsearch.Trim();
            var res = tsb.GetList(fullsearch, "", "", currentPageIndex, pageSize);
            return JsonNet(res);
        }


        public ActionResult GetException()
        {
            string a = "12a";
            int b = int.Parse(a);
            return Json(new { result = JsonConvert.SerializeObject(b) });
        }

        public JsonResult GetStatisticsData()
        {
            var res = tsb.GetStatisticsData();
            var category = tsb.getFullCategory();
            return Json(new { category = JsonConvert.SerializeObject(category), timesheet = JsonConvert.SerializeObject(res) });
        }

        public JsonResult GetUserHours(string dt)
        {
            var res = tsb.GetUserHours(dt);
            return Json(new { result = JsonConvert.SerializeObject(res) });
        }

        public JsonResult DeleteSummary(int summaryid)
        {
            int result = tsb.DeleteSummary(summaryid);
            return Json(new { result = result });
        }

        public JsonResult GetTimeSheet(int timesheetid)
        {
            var result = tsb.GetTimeSheet(timesheetid);
            return Json(new { result = JsonConvert.SerializeObject(result)});
        }

        public JsonResult DeleteTimeSheet(int timesheetid)
        {
            var result = tsb.DeleteTimeSheet(timesheetid);
            return Json(new { result = JsonConvert.SerializeObject(result) });
        }
    }
}