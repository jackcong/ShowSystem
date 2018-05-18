using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.DC
{
    public class CategoryDetail
    {
        [Key]
        public int Id { get; set; }
        public int HeaderId { get; set; }
        public string CategoryDetailName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }

        [ForeignKey("DetailId")]
        public virtual ICollection<CategoryCustomer> CategoryCustomers { get; set; }
    }
}
