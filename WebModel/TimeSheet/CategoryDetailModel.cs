using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.TimeSheet
{
    public class CategoryDetailModel
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }
        public string CategoryHeaderName { get; set; }
        public string CategoryDetailName { get; set; }
        public string CategoryCustomerName { get; set; }
        public int CategoryCustomerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public List<CategoryCustomerModel> CategoryCustomers { get; set; }
    }
}
