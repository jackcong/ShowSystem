using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Extension
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class HistoryAttribute : Attribute
    {
        public static readonly HistoryAttribute Default;
        private bool _needConvertToShortDate;
        private bool _isCheckBox;
        public HistoryAttribute()
        { 

        }
        /// <summary>
        /// This constructor is for some convert from datetime to date
        /// </summary>
        /// <param name="needConvertToShortDate"></param>
        public HistoryAttribute(bool needConvertToShortDate)
        {
            this._needConvertToShortDate = needConvertToShortDate;
        }
        public HistoryAttribute(string objectType)
        {
            if(objectType == "CheckBox")
                this._isCheckBox = true;
        }
        public virtual bool needConvertToShortDate { get { return this._needConvertToShortDate; } }
        public virtual bool isCheckBox { get { return this._isCheckBox; } }
    }
}
