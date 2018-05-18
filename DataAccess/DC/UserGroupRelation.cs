using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DataAccess.DC
{
    public class UserGroupRelation
    {
        [Key]
        public int Id { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
    }
}
