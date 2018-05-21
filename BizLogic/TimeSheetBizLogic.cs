
using ComLib.Exceptions;
using DataAccess.DC;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using WebModel.Account;
using WebModel.TimeSheet;

namespace BizLogic
{
    public class TimeSheetBizLogic : BaseBizLogic
    {
        TimeSheetManager tsm;
        UserModel _userModel;

        public TimeSheetBizLogic(UserModel um)
        {
            _userModel = um;
            tsm = new TimeSheetManager(um);
        }
       

        public object GetEditInfo()
        {
            return tsm.GetEditInfo();
        }

    }
}
