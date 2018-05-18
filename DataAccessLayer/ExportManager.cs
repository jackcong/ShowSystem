using DataAccess.DC;
using IDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WebModel.Account;

namespace DataAccessLayer
{
    public class ExportManager : BaseManager, IExportManager
    {
        private UserModel u;
        public ExportManager(UserModel user)
        {
            this.u = user;
        }
        public ExportManager()
        { }
        public DataTable GetTimeSheetExportData(string fullsearch, int userType)
        {
            try
            {
                TimeSheetManager timeSheetManager = new TimeSheetManager(this.u);
                return dc.ToDataTable(timeSheetManager.GetListDataForExport(fullsearch, userType).ToList());
            }
            catch (TimeoutException timeException)
            {
                throw timeException;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetTimeSheetDetailExportData(string fullsearch, int userType)
        {
            try
            {
                TimeSheetManager timeSheetManager = new TimeSheetManager(this.u);
                return dc.ToDataTable(timeSheetManager.GetListTimeSheetDetailDataForExport(fullsearch,userType).ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
