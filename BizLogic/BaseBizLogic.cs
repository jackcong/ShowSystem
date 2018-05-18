using WebModel.Account;

namespace BizLogic
{
    public class BaseBizLogic
    {
        protected UserModel u;
        protected string siteType;
        public BaseBizLogic(UserModel u)
        {
            this.u = u;
        }
        public BaseBizLogic()
        {
        }
    }
}
