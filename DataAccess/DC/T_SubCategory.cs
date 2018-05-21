namespace DataAccess.DC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class T_SubCategory
    {
        [Key]
        public int TID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(255)]
        public string SubCategoryName { get; set; }

        public string Content { get; set; }
    }
}
