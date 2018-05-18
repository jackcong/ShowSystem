using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.TimeSheet
{
    public class CategoryCustomerModel
    {
        public int Id { get; set; }
        public int DetailId { get; set; }
        public string CategoryCustomerName { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
    }
}
