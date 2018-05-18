using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.TimeSheet
{
    public class CategoryHeaderModel
    {
        public int Id { get; set; }
        public string CategoryHeaderName { get; set; }
        public int Seq { get; set; }
        public int? ShowStatistics { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        
        public List<CategoryDetailModel> CategoryDetails { get; set; }
    }
}
