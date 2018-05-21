using ComLib.Exceptions;
using ComLib.Extension;
using ComLib.SmartLinq;
using ComLib.SmartLinq.Energizer.JqGrid;
using DataAccess.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebModel.Account;
using WebModel.TimeSheet;

namespace DataAccessLayer
{
    public class TimeSheetManager : BaseManager
    {
        public TimeSheetManager(UserModel um)
        {
            this._user = um;
        }

        public List<T_Category> GetEditInfo()
        {
            List<T_Category> listCategory= dc.T_Category.ToList();

            if (listCategory != null)
            {
                foreach (T_Category category in listCategory)
                {
                    category.listSubCategory = dc.T_SubCategory.Where(c => c.CategoryID == category.TID).ToList();
                }
            }

            return listCategory;
        }
                
    }
}
