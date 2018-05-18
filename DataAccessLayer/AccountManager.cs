using ComLib.Security;
using DataAccess.DC;
using IDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WebModel.Account;
using ComLib.Mail;
using ComLib.Extension;

namespace DataAccessLayer
{
    public class AccountManager : BaseManager, IAccountManager
    {
        public DC dcObj;


        public int GetUserID(string email)
        {
            dcObj = DCLoader.GetMyDC();
            User user = dcObj.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                return user.ID;
            }
            else
            {
                return 0;
            }
        }

        public bool CheckEmailExists(string email)
        {
            dcObj = DCLoader.GetMyDC();
            var userCheck = dcObj.Users.FirstOrDefault(u => u.Email == email);
            if (userCheck == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int AddNewUser(string email)
        {
            dcObj = DCLoader.GetMyDC();
            User u = new User();
            u.Email = email;
            dcObj.Users.Add(u);
            dcObj.SaveChanges();
            return u.ID;
        }

        public UserModel GetCurrentUser(int userid)
        {
            dcObj = DCLoader.GetMyDC();
            UserModel userG = (from user in dcObj.Users where user.ID==userid
                         join gr in dcObj.UserGroupRelation on user.ID equals gr.UserID into leftResult
                         from result in leftResult.DefaultIfEmpty()
                         select new UserModel
                         {
                             ID = user.ID,
                             PassWord= user.PassWord,
                             UserName = user.UserName,
                             FirstName = user.FirstName,
                             LastName = user.LastName,
                             DisplayName = user.DisplayName,
                             NickName= user.NickName,
                             Email=user.Email,
                             Salt= user.Salt,
                             UserGroupID= (result==null?"":result.GroupID.ToString())
                         }
                        ).FirstOrDefault();

            if (userG != null)
            {
                return userG;
            }
            else
            {
                throw new Exception("Can not find user by id"+userid);
            }

        }

        public int ValidateUser(string userName, string passWord)
        {
            var user = new User();
            dcObj = DCLoader.GetMyDC();
            user = dcObj.Users.FirstOrDefault(c => c.UserName == userName && c.ActiveFlag == 1);

            if (user != null)
            {
                passWord = Hashing.HashWithSalt(passWord, user.Salt);
                if (user.PassWord == passWord)
                {
                    return 1;
                }
            }

            user = dcObj.Users.FirstOrDefault(c => c.UserName == userName);
            if (user != null && user.PassWord == passWord)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        public UserModel GetUserByName(string userName)
        {

            dcObj = DCLoader.GetMyDC();
            UserModel userG = (from user in dcObj.Users where user.UserName==userName
                               join gr in dcObj.UserGroupRelation on user.ID equals gr.UserID into leftResult
                               from result in leftResult.DefaultIfEmpty()
                               select new UserModel
                               {
                                   ID = user.ID,
                                   UserName = user.UserName,
                                   FirstName = user.FirstName,
                                   LastName = user.LastName,
                                   DisplayName = user.DisplayName,
                                   NickName = user.NickName,
                                   UserGroupID = (result == null ? "" : result.GroupID.ToString())
                               }
                        ).FirstOrDefault();
         
            return userG;
        }

        public bool ValidateOldPassword(UserModel user, string oldPassword)
        {
            string password = Hashing.HashWithSalt(oldPassword, user.Salt);
            if (password == user.PassWord)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdatePassword(UserModel user, string newPassword)
        {
            var passWord = Hashing.HashWithSalt(newPassword, user.Salt);
            User u = dc.Users.Where(c => c.ID == user.ID).FirstOrDefault();
            u.PassWord = passWord;
            dc.SaveChanges();
        }
    }
}
