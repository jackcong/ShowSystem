using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.TimeSheet
{
    public class AnalysisModel
    {
        public string DisplayName { get; set; }
        public int ID { get; set; }

        public string GroupName { get; set; }
        public DateTime? DateOpened { get; set; }
        public decimal TotalHours { get; set; }
    }
}
