using ComLib.Extension;
using ComLib.Mail;
using DataAccess.DC;
using DataAccessLayer;
using IDataAccessLayer;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using WebModel.Account;
using System;
using System.Web;

namespace BizLogic
{
    public class AccountHelper
    {
        public static bool CheckEmailExists(string email)
        {
            IAccountManager IAccount = new AccountManager();
            return IAccount.CheckEmailExists(email);
        }

        public static int GetUserID(string email)
        {
            IAccountManager IAccount = new AccountManager();
            return IAccount.GetUserID(email);
        }

        public static UserModel GetCurrentUser()
        {
            if (HttpContext.Current.Request.Cookies["UserID"] != null)
            {
                int userid = int.Parse(HttpContext.Current.Request.Cookies["UserID"].Value.ToString());
                IAccountManager IAccount = new AccountManager();
                return IAccount.GetCurrentUser(userid);
            }
            else
            {
                throw new Exception("Can not find user id' cookies");
            }
        }

        public int UserLogon(string userName, string password)
        {
            IAccountManager IAccount = new AccountManager();
            return IAccount.ValidateUser(userName, password);
        }

        public UserModel GetUserByName(string userName)
        {
            IAccountManager IAccount = new AccountManager();
            return IAccount.GetUserByName(userName);
        }

        public static int AddNewUser(string email)
        {
            IAccountManager IAccount = new AccountManager();
            return IAccount.AddNewUser(email);
        }

        public bool ValidateOldPassword(string oldPassword)
        {
            IAccountManager IAccount = new AccountManager();
            UserModel user = GetCurrentUser();
            return IAccount.ValidateOldPassword(user,oldPassword);
        }

        public void UpdatePassword(string newPassword)
        {
            IAccountManager IAccount = new AccountManager();
            UserModel user = GetCurrentUser();
            IAccount.UpdatePassword(user,newPassword);
        }
    }
}
