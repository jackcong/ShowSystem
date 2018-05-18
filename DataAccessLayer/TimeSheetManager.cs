using ComLib.Exceptions;
using ComLib.Extension;
using ComLib.SmartLinq;
using ComLib.SmartLinq.Energizer.JqGrid;
using DataAccess.DC;
using IDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebModel.Account;
using WebModel.TimeSheet;

namespace DataAccessLayer
{
    public class TimeSheetManager : BaseManager,ITimeSheetManager
    {
        public TimeSheetManager(UserModel um)
        {
            this._user = um;
        }

        public object GetList(string fullsearch, string username, string permissions, int currentPageIndex, int pageSize)
        {
            return this.GetList(fullsearch, currentPageIndex, pageSize);
        }

        private object GetList(string fullsearch, int currentPageIndex, int pageSize)
        {
            var data = this.GetListData(fullsearch);
            var res = data.Skip((currentPageIndex - 1) * pageSize).Take(pageSize).ToList();

            int count = data.Count();

            return new
            {
                total = pageSize > 0 ? Math.Ceiling((double)count / pageSize) : 1,
                page = currentPageIndex,
                records = count,
                rows = res.ToArray()
            };
        }

        public int CheckSummaryExists(int year,int week,int detailid)
        {
            try
            {
                var obj = dc.SummaryTimeSheets.Where(c => c.UserID == _user.ID && c.TypeYear==year && (c.TypeWeek==week) && c.Id != detailid).FirstOrDefault();
                if (obj != null)
                {
                    return obj.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int saveNodeHeader(int id, string nodeName)
        {
            CategoryHeader ch =dc.CategoryHeaders.Where(c => c.Id == id).FirstOrDefault();
            if (ch != null)
            {
                ch.CategoryHeaderName = nodeName;
                dc.SaveChanges();
                return ch.Id;
            }
            return 0;
        }

        public int saveNodeDetail(int id, string nodeName)
        {
            CategoryDetail cd= dc.CategoryDetails.Where(c => c.Id == id).FirstOrDefault();
            if (cd != null)
            {
                cd.CategoryDetailName = nodeName;
                dc.SaveChanges();
                return cd.Id;
            }
            return 0;
        }

        public int deleteCustomerNode(int id)
        {
            CategoryCustomer cc = dc.CategoryCustomers.Where(c => c.Id == id).FirstOrDefault();
            if (cc != null)
            {
                dc.CategoryCustomers.Remove(cc);
                dc.SaveChanges();
                return 1;
            }
            return 0;
        }

        public int deleteDetailNode(int id)
        {
            CategoryDetail cd= dc.CategoryDetails.Where(c => c.Id == id).FirstOrDefault();
            if (cd != null)
            {
                dc.CategoryDetails.Remove(cd);
                dc.SaveChanges();
                return 1;
            }
            return 0;
        }

        public int saveNodeCustomer(int id, string nodeName)
        {
            CategoryCustomer cc = dc.CategoryCustomers.Where(c => c.Id == id).FirstOrDefault();
            if (cc != null)
            {
                cc.CategoryCustomerName = nodeName;
                dc.SaveChanges();
                return cc.Id;
            }
            return 0;
        }

        public bool checkDetailNameExists(string nodeName)
        {
            CategoryDetail cd = dc.CategoryDetails.Where(c => c.CategoryDetailName == nodeName).FirstOrDefault();
            if (cd == null)
            {
                return false;
            }
            return true;
        }

        public decimal GetTotalHourByWeek(int lastWeek)
        {
            SummaryTimeSheet sts = dc.SummaryTimeSheets.Where(c => c.UserID == _user.ID && c.TypeWeek == lastWeek).FirstOrDefault();
            if (sts != null)
            {
                return sts.TotalHours;
            }
            else
            {
                return 40;
            }
        }

        public bool checkCustomerNameExists(string nodeName)
        {
            CategoryCustomer cc = dc.CategoryCustomers.Where(c => c.CategoryCustomerName == nodeName).FirstOrDefault();
            if (cc == null)
            {
                return false;
            }
            return true;

        }

        public int saveCustomerNode(int id, string nodeName)
        {
            CategoryCustomer cc = new CategoryCustomer();
            cc.DetailId = id;
            cc.CategoryCustomerName = nodeName;
            cc.CreatedDate = DateTime.Now.ToUniversalTime();
            cc.CreatedUser = _user.DisplayName;
            dc.CategoryCustomers.Add(cc);
            dc.SaveChanges();
            return cc.Id;
        }

        public int saveDetailNode(int id, string nodeName)
        {
            CategoryDetail cd = new CategoryDetail();
            cd.HeaderId = id;
            cd.CategoryDetailName = nodeName;
            cd.CreatedDate = DateTime.Now.ToUniversalTime();
            cd.CreatedUser = _user.DisplayName;

            dc.CategoryDetails.Add(cd);
            dc.SaveChanges();
            return cd.Id;
        }

        public string GetTimeSheetComment(int timesheetid)
        {
            string strComment = dc.TimeSheets.Where(c => c.Id == timesheetid).Select(c => c.Comment).FirstOrDefault();
            if (string.IsNullOrEmpty(strComment))
            {
                strComment = "";
            }
            strComment = strComment.Replace("\r", "</br>").Replace("\n", "</br>");
            return strComment;
        }

        public SummaryTimeSheetModel CreateNewSummaryModel()
        {

            var groupname = (from g in dc.UserGroupRelation
                             join ug in dc.UserGroups on g.GroupID equals ug.Id
                             where g.UserID ==_user.ID
                             select ug.GroupName
                             ).FirstOrDefault();

            SummaryTimeSheetModel stsModel = new SummaryTimeSheetModel();

            DateTime dtNow = DateTime.Now;

            stsModel.TypeYear = dtNow.Year;

            int firstWeekend = 7 - Convert.ToInt32(DateTime.Parse(DateTime.Today.Year + "-1-1").DayOfWeek);
            int currentDay = DateTime.Today.DayOfYear;
            stsModel.DateOpened = null;
            stsModel.TotalHours = 0;
            stsModel.DisplayName = _user.DisplayName;
            stsModel.UserID = _user.ID;
            stsModel.Email = _user.Email;
            stsModel.GroupName = groupname;
            stsModel.Id = 0;
            var TSID = dc.SummaryTimeSheets.Where(c => c.UserID == _user.ID).Select(c => c.TSID).ToList();
            if (TSID.Count==0)
            {
                stsModel.TSID = 1;
            }
            else
            {
                stsModel.TSID = TSID.Max()+1;
            }

            stsModel.listTimeSheet = new List<TimeSheetModel>();

            return stsModel;
        }

        public int SaveSummary(SummaryTimeSheetModel stsm)
        {
            try
            {
                SummaryTimeSheet sts = new SummaryTimeSheet();
                ModelConverter.Convert(stsm, sts);
                sts.CreatedDate = DateTime.Now;
                dc.SummaryTimeSheets.Add(sts);
                dc.SaveChanges();
                return sts.Id;
            }
            catch (System.NullReferenceException nr)
            {
                throw new Exception("Null reference object ,please try again.", nr);
            }
            catch (System.TimeoutException te)
            {
                throw new Exception("Timeout ,please refresh page.",te);
            }
            catch (System.InvalidOperationException ioe)
            {
                throw new Exception("Database invalid ,please try again.", ioe);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public object DeleteTimeSheet(int timesheetid)
        {
            try
            {
                TimeSheet ts = dc.TimeSheets.Where(c => c.Id == timesheetid).FirstOrDefault();
                if (ts != null)
                {
                    SummaryTimeSheet sts = dc.SummaryTimeSheets.Where(c => c.Id == ts.SummaryID).FirstOrDefault();
                    sts.TotalHours -= ts.ActHours;

                    dc.TimeSheets.Remove(ts);
                    dc.SaveChanges();
                }

                return 1;
            }
            catch (System.NullReferenceException nr)
            {
                throw new Exception("Null reference object ,please try again.", nr);
            }
            catch (System.TimeoutException te)
            {
                throw new Exception("Timeout ,please refresh page.",te);
            }
            catch (System.InvalidOperationException ine)
            {
                throw new Exception("Database invalid ,please try again.", ine);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public object GetTimeSheet(int timesheetid)
        {
            try
            {
                TimeSheetModel tsm = new TimeSheetModel();
                TimeSheet ts = dc.TimeSheets.Where(c => c.Id == timesheetid).FirstOrDefault();
                if (ts != null)
                {
                    ModelConverter.Convert(ts, tsm);
                    return tsm;
                }
                return tsm;
            }
            catch (System.NullReferenceException)
            {
                throw new Exception("Null reference object ,please try again.");
            }
            catch (System.TimeoutException)
            {
                throw new Exception("Timeout ,please refresh page.");
            }
            catch (System.InvalidOperationException)
            {
                throw new Exception("Database invalid ,please try again.");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteSummary(int summaryid)
        {
            try
            {
                dc.Database.ExecuteSqlCommand("delete from timesheet where SummaryID=" + summaryid);
                dc.Database.ExecuteSqlCommand("delete from summarytimesheet where Id=" + summaryid);
                return 1;
            }
            catch (System.NullReferenceException)
            {
                throw new Exception("Null reference object ,please try again.");
            }
            catch (System.TimeoutException)
            {
                throw new Exception("Timeout ,please refresh page.");
            }
            catch (System.InvalidOperationException)
            {
                throw new Exception("Database invalid ,please try again.");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public SummaryTimeSheetModel GetSummaryAndTimeSheet(int detailID)
        {
            try
            {
                SummaryTimeSheet sts = dc.SummaryTimeSheets.Include("listTimeSheet").Where(c => c.Id == detailID).FirstOrDefault();
                if (sts.listTimeSheet.Count > 0)
                {
                    sts.listTimeSheet = sts.listTimeSheet.OrderByDescending(c => c.CreatedDate).ToList();
                }
                else
                {
                    sts.listTimeSheet = new List<TimeSheet>();
                }

                SummaryTimeSheetModel stsm = new SummaryTimeSheetModel();
                ModelConverter.Convert(sts, stsm);
                stsm.listTimeSheet = new List<TimeSheetModel>();

                foreach (TimeSheet ts in sts.listTimeSheet)
                {
                    TimeSheetModel tsm = new TimeSheetModel();
                    ModelConverter.Convert(ts, tsm);
                    stsm.listTimeSheet.Add(tsm);
                }

                stsm.listTimeSheet = stsm.listTimeSheet.OrderBy(c => c.TypeDate).ToList();

                return stsm;
            }
            catch (System.NullReferenceException nr)
            {
                throw new Exception("Null reference object ,please try again.", nr);
            }
            catch (System.TimeoutException te)
            {
                throw new Exception("Timeout ,please refresh page.",te);
            }
            catch (System.InvalidOperationException ioe)
            {
                throw new Exception("Database invalid ,please try again.", ioe);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetStatisticsData()
        {
             return dc.TimeSheets.ToList();
        }

        public IQueryable<SummaryTimeSheet> GetListData(string fullsearch)
        {
            try
            {
                if (!string.IsNullOrEmpty(fullsearch))
                {
                    //5 is admin ,so show all data.
                    if (_user.UserGroupID == "5")
                    {
                        var res = dc.SummaryTimeSheets.Include("user")
                            .Where(c => (c.DisplayName.Contains(fullsearch) || c.GroupName.Contains(fullsearch) || c.YearAndWeek.Contains(fullsearch)))
                            .OrderBy(c => c.DisplayName)
                            .ThenByDescending(m => m.DateOpened).AsQueryable();
                        return res;
                    }
                    else
                    {
                        var res = dc.SummaryTimeSheets.Include("user").Where(c => (c.DisplayName.Contains(fullsearch) || c.GroupName.Contains(fullsearch) || c.YearAndWeek.Contains(fullsearch))).OrderBy(c => (c.UserID == _user.ID ? 0 : 1)).ThenBy(c => c.UserID).ThenByDescending(m => m.DateOpened).AsQueryable();
                        return res;
                    }
                }
                else
                {
                    if (_user.UserGroupID == "5")
                    {
                        var res = dc.SummaryTimeSheets.Include("user").OrderBy(c => c.DisplayName).ThenByDescending(m => m.DateOpened).AsQueryable();
                        return res;
                    }
                    else
                    {
                        var res = dc.SummaryTimeSheets.Include("user").OrderBy(c => (c.UserID == _user.ID ? 0 : 1)).ThenBy(c => c.UserID).ThenByDescending(m => m.DateOpened).AsQueryable();
                        return res;
                    }
                }
            }
            catch (System.NullReferenceException nr)
            {
                throw new Exception("Null reference object ,please try again.",nr);
            }
            catch (System.TimeoutException te)
            {
                throw new Exception("Timeout ,please refresh page.",te);
            }
            catch (System.InvalidOperationException ioe)
            {
                throw new Exception("Database invalid ,please try again.", ioe);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<SummaryTimeSheet> GetListDataForExport(string fullsearch, int userType)
        {

            if (!string.IsNullOrEmpty(fullsearch))
            {
                //current user.
                if (userType == 1)
                {
                    var res = dc.SummaryTimeSheets.Include("user").Where(c => c.UserID == _user.ID && (c.DisplayName.Contains(fullsearch) || c.GroupName.Contains(fullsearch))).OrderByDescending(m => m.DateOpened).AsQueryable();
                    return res;
                }
                else
                {
                    var res = dc.SummaryTimeSheets.Include("user").Where(c => (c.DisplayName.Contains(fullsearch) || c.GroupName.Contains(fullsearch))).OrderBy(c=>c.UserID).ThenByDescending(c => c.DateOpened).AsQueryable();
                    return res;
                }
                
            }
            else
            {
                if (userType == 1)
                {
                    var res = dc.SummaryTimeSheets.Include("user").Where(c => c.UserID == _user.ID).OrderByDescending(m => m.DateOpened).AsQueryable();
                    return res;
                }
                else
                {
                    var res = dc.SummaryTimeSheets.Include("user").OrderBy(c=>c.UserID).ThenByDescending(m => m.DateOpened).AsQueryable();
                    return res;
                }
            }
        }


        public IQueryable<TimeSheet> GetListTimeSheetDetailDataForExport(string fullsearch, int userType)
        {
            if (!string.IsNullOrEmpty(fullsearch))
            {
                if (userType == 1)
                {
                    var res = dc.TimeSheets.Include("user").Where(c => c.UserID == _user.ID && (c.CategoryDetailName.Contains(fullsearch) || c.Comment.Contains(fullsearch))).OrderByDescending(m => m.TypeDate).AsQueryable();
                    return res;
                }
                else
                {
                    var res = dc.TimeSheets.Include("user").Where(c =>(c.CategoryDetailName.Contains(fullsearch) || c.Comment.Contains(fullsearch))).OrderBy(c=>c.UserID).ThenByDescending(m => m.TypeDate).AsQueryable();
                    return res;
                }
            }
            else
            {
                if (userType == 1)
                {
                    var res = dc.TimeSheets.Include("user").Where(c => c.UserID == _user.ID).OrderByDescending(m => m.TypeDate).AsQueryable();
                    return res;
                }
                else
                {
                    var res = dc.TimeSheets.Include("user").OrderBy(c=>c.UserID).ThenByDescending(m => m.TypeDate).AsQueryable();
                    return res;
                }
            }
        }

        public List<CategoryDetailModel> GetCategory()
        {
            try
            {
                List<CategoryHeader> listCategory = dc.CategoryHeaders.Include("CategoryDetails").Include("CategoryDetails.CategoryCustomers").ToList();

                List<CategoryDetailModel> listCategoryModel = new List<CategoryDetailModel>();

                foreach (CategoryHeader ch in listCategory)
                {
                    CategoryHeaderModel chm = new CategoryHeaderModel();

                    ModelConverter.Convert(ch, chm);
                    chm.CategoryDetails = new List<CategoryDetailModel>();

                    if (ch.CategoryDetails != null)
                    {
                        foreach (CategoryDetail cd in ch.CategoryDetails)
                        {
                            if (cd.CategoryCustomers != null && cd.CategoryCustomers.Count > 0)
                            {
                                foreach (CategoryCustomer cc in cd.CategoryCustomers)
                                {
                                    CategoryDetailModel cdm = new CategoryDetailModel();
                                    ModelConverter.Convert(cd, cdm);
                                    cdm.CategoryHeaderName = chm.CategoryHeaderName;
                                    cdm.CategoryCustomerName = cc.CategoryCustomerName;
                                    cdm.CategoryCustomerId = cc.Id;
                                    listCategoryModel.Add(cdm);
                                }
                            }
                            else
                            {
                                CategoryDetailModel cdm = new CategoryDetailModel();
                                ModelConverter.Convert(cd, cdm);
                                cdm.CategoryHeaderName = chm.CategoryHeaderName;
                                cdm.CategoryCustomerName = "";
                                cdm.CategoryCustomerId = 0;
                                listCategoryModel.Add(cdm);
                            }
                        }
                    }
                }
                return listCategoryModel;
            }
            catch (NullReferenceException nullpoint)
            {
                throw nullpoint;
            }
        }

        public List<CategoryHeaderModel> GetFullCategory()
        {
            List<CategoryHeader> listCategory = dc.CategoryHeaders.Include("CategoryDetails").Include("CategoryDetails.CategoryCustomers").Where(c => c.ShowStatistics == 1).OrderBy(c=>c.Seq).ToList();

            List<CategoryHeaderModel> listCategoryModel = new List<CategoryHeaderModel>();

            foreach (CategoryHeader ch in listCategory)
            {
                CategoryHeaderModel chm = new CategoryHeaderModel();

                ModelConverter.Convert(ch, chm);
                chm.CategoryDetails = new List<CategoryDetailModel>();

                if (ch.CategoryDetails != null)
                {
                    foreach (CategoryDetail cd in ch.CategoryDetails)
                    {
                        CategoryDetailModel cdm = new CategoryDetailModel();
                        ModelConverter.Convert(cd, cdm);
                        cdm.CategoryHeaderName = chm.CategoryHeaderName;

                        cdm.CategoryCustomers = new List<CategoryCustomerModel>();

                        if (cd.CategoryCustomers != null)
                        {
                            foreach (CategoryCustomer cc in cd.CategoryCustomers)
                            {
                                CategoryCustomerModel ccm = new CategoryCustomerModel();
                                ModelConverter.Convert(cc, ccm);

                                cdm.CategoryCustomers.Add(ccm);
                            }
                        }

                        chm.CategoryDetails.Add(cdm);
                    }
                }

                listCategoryModel.Add(chm);
            }

            return listCategoryModel;
        }

        public int SaveTimeSheet(TimeSheetModel tsm)
        {
            try
            {
                TimeSheet ts;
                if (tsm.Id != 0)
                {
                    ts = dc.TimeSheets.Where(c => c.Id == tsm.Id).FirstOrDefault();
                    ModelConverter.Convert(tsm, ts);
                    ts.CreatedDate = DateTime.Now;

                    SummaryTimeSheet sts = dc.SummaryTimeSheets.Where(c => c.Id == tsm.SummaryID).FirstOrDefault();

                    if (sts != null)
                    {
                        sts.TypeWeek = tsm.TypeWeek;
                        sts.TotalHours -= tsm.OldActHours;
                        sts.TotalHours += tsm.ActHours;
                        sts.YearAndWeek = sts.TypeDate.Value.Year.ToString() + "W" + sts.TypeWeek.Value.ToString("00");
                    }
                }
                else
                {
                    ts = new TimeSheet();
                    ModelConverter.Convert(tsm, ts);
                    ts.CreatedDate = DateTime.Now;
                    ts.UserID = _user.ID;
                    ts.CategoryName = tsm.CategoryDetailName.Split('-')[0];
                    ts.DetailName = tsm.CategoryDetailName.Split('-')[1];
                    ts.TypeYear = ts.CreatedDate.Year;
                    dc.TimeSheets.Add(ts);

                    SummaryTimeSheet sts = dc.SummaryTimeSheets.Where(c => c.Id == tsm.SummaryID).FirstOrDefault();

                    if (sts != null)
                    {
                        sts.TypeWeek = tsm.TypeWeek;
                        sts.TotalHours += tsm.ActHours;
                        sts.YearAndWeek = sts.TypeDate.Value.Year.ToString() + "W" + sts.TypeWeek.Value.ToString("00");
                    }
                }

                return dc.SaveChanges();
            }
            catch (System.NullReferenceException)
            {
                throw new Exception("Null reference object ,please try again.");
            }
            catch (System.FormatException)
            {
                throw new Exception("Format incorrent,please check.");
            }
            catch (System.InvalidCastException)
            {
                throw new Exception("Cast error.,please check.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AnalysisModel> GetUserHours(string dt)
        {
            try
            {
                string[] strDate = dt.Split('/');

                DateTime dtInner = new DateTime(int.Parse(strDate[2]), int.Parse(strDate[1]), int.Parse(strDate[0]));

                List<AnalysisModel> listAnalysisModel = dc.Database.SqlQuery<AnalysisModel>("select * from SearchHoursByDate('" + dtInner.ToShortDateString() + "') order by DisplayName asc").ToList();
                return listAnalysisModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
