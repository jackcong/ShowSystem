using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataAccess.DC
{
    public class CategoryHeader
    {
        [Key]
        public int Id { get; set; }
        public string CategoryHeaderName { get; set; }
        public int Seq { get; set; }
        public int? ShowStatistics { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }

        [ForeignKey("HeaderId")]
        public virtual ICollection<CategoryDetail> CategoryDetails { get; set; }
    }
}
