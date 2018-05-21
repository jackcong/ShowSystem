namespace DataAccess.DC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class T_Category
    {
        [Key]
        public int TID { get; set; }

        [StringLength(255)]
        public string CategoryName { get; set; }

        [ForeignKey("CategoryID")]
        public virtual ICollection<T_SubCategory> listSubCategory { get; set; }
    }
}
