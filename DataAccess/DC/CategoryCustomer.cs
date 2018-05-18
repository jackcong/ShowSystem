using System;

using System.ComponentModel.DataAnnotations;


namespace DataAccess.DC
{
    public class CategoryCustomer
    {
        [Key]
        public int Id { get; set; }
        public int DetailId { get; set; }
        public string CategoryCustomerName { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
    }
}
