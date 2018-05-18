using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace ComLib.Extension
{
    public class ModelAttrExtension
    {
        public Hashtable ModelAttr(Type model,string[] fieldList)
        {
            Hashtable fieldInfo = new Hashtable();
            foreach(string attr in fieldList)
            {
                string attrDisplayName = ModelAttr(model, attr);
                fieldInfo.Add(attr,attrDisplayName);
            }
            return fieldInfo ;
        }
        public string ModelAttr(Type model, string fieldName)
        {
            string attrDisplayName = (TypeDescriptor.GetProperties(model)[fieldName].Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute).DisplayName;
            return attrDisplayName;
        }
    }
}
