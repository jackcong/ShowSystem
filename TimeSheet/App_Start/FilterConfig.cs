using System.Web;
using System.Web.Mvc;
using TimeSheet.App_Start;

namespace TimeSheet
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}
