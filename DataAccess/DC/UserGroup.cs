using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.DC
{
    public class UserGroup
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedUser { get; set; }
    }
}
