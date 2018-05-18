using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataAccess.DC
{
    public class SummaryTimeSheet
    {
        [Key]
        public int Id { get; set; }
        public int? TypeYear { get; set; }
        public int? TypeWeek { get; set; }
        public DateTime? TypeDate { get; set; }
        public string YearAndWeek { get; set; }
        public string GroupName { get;set;}
        public string CategoryCustomerName { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? EndDayOfWeek { get; set; }
        public string DisplayName { get; set; }
        public int UserID { get; set; }
        public string Email { get; set; }
        public DateTime DateOpened { get; set; }
        public decimal TotalHours { get; set; }
        public string CategoryDetailName { get; set; }

        public DateTime CreatedDate { get; set; }

        public int TSID { get; set; }

        [NotMapped]
        public string TSIDS { get { return Id.ToString("000000"); } }

        [ForeignKey("SummaryID")]
        public virtual ICollection<TimeSheet> listTimeSheet { get; set; }

        [ForeignKey("UserID")]
        public User user { get; set; }
    }
}
