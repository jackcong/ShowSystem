using DataAccessLayer;
using IDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WebModel.Account;

namespace BizLogic
{
    public class ExportBizLogic
    {
        IExportManager exportManager;// = new ExportManager();
        UserModel u;
        public ExportBizLogic(UserModel u)
        {
            exportManager = new ExportManager(u);
            this.u = u;
        }

        public DataTable GetTimeSheetExportData(string fullsearch, int userType)
        {
            return exportManager.GetTimeSheetExportData(fullsearch,userType);
        }
        public DataTable GetTimeSheetDetailExportData(string fullsearch, int userType)
        {
            return exportManager.GetTimeSheetDetailExportData(fullsearch,userType);
        }
    }
}
