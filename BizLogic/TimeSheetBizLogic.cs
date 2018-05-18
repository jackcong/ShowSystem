
using ComLib.Exceptions;
using DataAccess.DC;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using WebModel.Account;
using WebModel.TimeSheet;

namespace BizLogic
{
    public class TimeSheetBizLogic : BaseBizLogic
    {
        TimeSheetManager tsm;
        UserModel _userModel;

        public TimeSheetBizLogic(UserModel um)
        {
            _userModel = um;
            tsm = new TimeSheetManager(um);
        }

        public object GetCategory()
        {
            object returnData = CallMethod(new Func<object>(tsm.GetCategory));
            return returnData;
        }

        public object getFullCategory()
        {
            object returnData = CallMethod(new Func<object>(tsm.GetFullCategory));
            return returnData;
        }

        public object CallMethod<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (System.NullReferenceException nre)
            {
                throw new Exception("Null reference object ,please try again.",nre);
            }
            catch (System.FormatException fe)
            {
                throw new Exception("Format incorrent,please check.",fe);
            }
            catch (System.InvalidCastException ice)
            {
                throw new Exception("Cast error,please check.", ice);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int saveNode(int id, string level, string nodeName)
        {
            if (level == "header")
            {
                return tsm.saveNodeHeader(id,nodeName);
            }
            else if (level == "detail")
            {
                return tsm.saveNodeDetail(id, nodeName);
            }
            else if (level == "customer")
            {
                return tsm.saveNodeCustomer(id, nodeName);
            }
            else
            {
                return 0;
            }
            
        }

        public int deleteNode(int id, string level)
        {
            if (level == "detail")
            {
                return tsm.deleteDetailNode(id);
            }
            else if (level == "customer")
            {
                return tsm.deleteCustomerNode(id);
            }
            else
            {
                return 0;
            }
        }

        public bool checkNodeNameExists(string level, string nodeName)
        {
            if (level == "header")
            {
                return tsm.checkDetailNameExists(nodeName);
            }
            else if (level == "detail")
            {
                return tsm.checkCustomerNameExists(nodeName);
            }
            else {
                return false;
            }
        }

        public int saveNewNode(int id, string level, string nodeName)
        {
            if (level == "header")
            {
                return tsm.saveDetailNode(id, nodeName);
            }

            if (level == "detail")
            {
                return tsm.saveCustomerNode(id, nodeName);
            }
            return 0;
        }

        public int SaveTimeSheet(SummaryTimeSheetModel stsm,TimeSheetModel timesheetm)
        {
            try
            {
                if (stsm.Id == 0)
                {
                    int lastWeek = timesheetm.TypeWeek.Value - 1;

                    //get last week total hour.

                    //decimal dTotalHour = tsm.GetTotalHourByWeek(lastWeek);

                    //last week need 40 hours
                    //if (dTotalHour < 40)
                    //{
                    //    return -1;
                    //}

                    //need save
                    stsm.TypeDate = timesheetm.TypeDate;
                    stsm.DateOpened = timesheetm.TypeDate;

                    int weeknow = Convert.ToInt32(stsm.DateOpened.Value.DayOfWeek);
                    int daydiff = (6 - weeknow);
                    DateTime lastDay = stsm.DateOpened.Value.AddDays(daydiff);
                    stsm.EndDayOfWeek = lastDay;

                    int summaryId = tsm.SaveSummary(stsm);
                    timesheetm.SummaryID = summaryId;
                }
                tsm.SaveTimeSheet(timesheetm);
                return timesheetm.SummaryID;
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
            catch (TimeoutException timeex)
            {
                throw timeex;
            }
            catch (ArgumentException argumentex)
            {
                throw argumentex;
            }
        }

        public object GetList(string fullsearch, string username, string permissions, int currentPageIndex, int pageSize)
        {
            return tsm.GetList(fullsearch, username, permissions, currentPageIndex, pageSize);
        }

        public SummaryTimeSheetModel GetSummaryAndTimeSheet(int detailID)
        {
            SummaryTimeSheetModel stsm;
            if (detailID == 0)
            {
                stsm = tsm.CreateNewSummaryModel();
                return stsm;
            }

            return tsm.GetSummaryAndTimeSheet(detailID);
        }

        public string GetTimeSheetComment(int timesheetid)
        {
            return tsm.GetTimeSheetComment(timesheetid);
        }

        public int CheckSummaryExists(int year,int week,int detailid)
        {
            return tsm.CheckSummaryExists(year,week,detailid);
        }

        public object GetStatisticsData()
        {
            return tsm.GetStatisticsData();
        }

        public object GetUserHours(string DateTime)
        {
            return tsm.GetUserHours(DateTime);
        }

        public int DeleteSummary(int summaryid)
        {
            return tsm.DeleteSummary(summaryid);
        }

        public object GetTimeSheet(int timesheetid)
        {
            return tsm.GetTimeSheet(timesheetid);
        }

        public object DeleteTimeSheet(int timesheetid)
        {
            return tsm.DeleteTimeSheet(timesheetid);
        }
    }
}
