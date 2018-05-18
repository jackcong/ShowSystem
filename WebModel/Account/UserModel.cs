using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.Account
{
    public class UserModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string NickName { get; set; }

        public string Title { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string TimeZone { get; set; }

        public int? ActiveFlag { get; set; }

        public string UserGroupID { get; set; }

        public string Salt { get; set; }
    }
}
