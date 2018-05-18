using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace IDataAccessLayer
{
    public interface IExportManager
    {
        DataTable GetTimeSheetExportData(string fullsearch, int userType);
        DataTable GetTimeSheetDetailExportData(string fullsearch, int userType);
    }
}
