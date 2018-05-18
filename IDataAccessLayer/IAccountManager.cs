using DataAccess.DC;
using System.Collections.Generic;
using WebModel.Account;
namespace IDataAccessLayer
{
    public interface IAccountManager
    {
        int GetUserID(string email);
        
        bool CheckEmailExists(string email);
        int AddNewUser(string email);
        UserModel GetCurrentUser(int userid);
        int ValidateUser(string userName, string password);
        UserModel GetUserByName(string userName);
        bool ValidateOldPassword(UserModel user, string oldPassword);
        void UpdatePassword(UserModel user, string newPassword);
    }
}
