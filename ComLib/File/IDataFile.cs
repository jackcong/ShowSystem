using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ComLib.File
{
    public interface IDataFile
    {
        DataTable ToDataTable();
    }
}
