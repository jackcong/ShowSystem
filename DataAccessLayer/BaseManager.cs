using DataAccess.DC;
using WebModel.Account;

namespace DataAccessLayer
{
    /// <summary>
    /// Base class for database imp.
    /// </summary>
    public class BaseManager
    {
        public DC dc;
        protected UserModel _user;
        private readonly object _saveLock = new object();

        public BaseManager()
        {
            dc = DCLoader.GetMyDC();
        }
    }
}
