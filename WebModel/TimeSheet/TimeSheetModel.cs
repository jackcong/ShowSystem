using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebModel.Account;

namespace WebModel.TimeSheet
{
    public class TimeSheetModel
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int CategoryCustomerID { get; set; }
        public string CategoryCustomerName { get; set; }
        public int CategoryDetailID { get; set; }
        public string CategoryDetailName { get; set; }

        public string CategoryName { get; set; }
        public string DetailName { get; set; }

        public decimal ActHours { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime TypeDate { get; set; }
        public int? TypeWeek { get; set; }
        public int? TypeYear { get; set; }

        public int SummaryID { get; set; }

        public decimal OldActHours { get; set; }

        //public DateTime EndDayOfWeek
        //{
        //    get
        //    {
        //        //Set saturday is the last day of week
        //        int weeknow = Convert.ToInt32(TypeDate.DayOfWeek);
        //        int daydiff = (7 - weeknow) - 1;

        //        //Last day of this week
        //        string LastDay = TypeDate.AddDays(daydiff).ToString("yyyy-MM-dd");
        //        return Convert.ToDateTime(LastDay);
        //    }
        //}

        public UserModel user { get; set; }

    }
}
