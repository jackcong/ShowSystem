
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.DC
{
    public class TimeSheet
    {
        [Key]
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
        public string WeekToShow { get { return this.TypeYear + "W" + this.TypeWeek; } }
        [ForeignKey("UserID")]
        public User user { get; set; }

        [NotMapped]
        public string DisplayName { get { return this.user==null?"":this.user.DisplayName; } }

        public int SummaryID { get; set; }
    }
}
