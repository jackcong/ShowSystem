using System.Linq;
using ComLib.Extension;

namespace ComLib.File.Excel
{
    public static class ExcelDataExtension
    {
        public static ExcelData ToExcelFile<T>(this T[] param, string[] orderList, string[] headList = null, string sheetName = null) where T : class
        {
            T t;
            string[] head = headList ?? orderList;
            string[,] data = new string[param.Count(), orderList.Count()];
            for (int i = 0; i < param.Count(); ++i)
            {
                for (int j = 0; j < orderList.Count(); ++j)
                {
                    // TODO: Make this flexible for display format.
                    object value = typeof(T).GetProperty(orderList[j]).GetValue(param[i], null);
                    // TODO: Come up with an elegant solution to Find nasty characters that can break your excel file
                    data[i, j] = value == null
                                     ? ""
                                     : value.GetType().GetUnderlyingType() == typeof (decimal)
                                           ? ((decimal) value).ToString("N")
                                           : value.ToString().Replace("<br>", "\n");
                }
            }
            ExcelData excel = new ExcelData();
            excel.BuildHead(head);
            excel.BuildBody(data);
            if (sheetName != null)
                excel.SheetName = sheetName;
            return excel;
        }
    }
}
