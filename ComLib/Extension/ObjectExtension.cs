using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ComLib.Extension
{
    public static class ObjectExtension
    {
        public static object[] PropertiesToArray(this object obj)
        {
            return obj.GetType().GetProperties().Select(pinfo => pinfo.GetGetMethod().Invoke(obj, null)).ToArray();
        }

        public static PropertyInfo[] PropertiesToStringList(this object obj)
        {
            return obj.GetType().GetProperties();
        }

        public static object[] PropertiesToJqGridArray(this object obj)
        {
            var result = new List<string>();
            foreach (var propertyInfo in obj.GetType().GetProperties())
            {
                var value = propertyInfo.GetGetMethod().Invoke(obj, null);
                if (value == null)
                {
                    result.Add(string.Empty);
                    continue;
                }

                var propType = propertyInfo.GetType();
                if (propType.Equals(typeof(DateTime)) || propType.Equals(typeof(DateTime?)) )
                {
                    result.Add(((DateTime)value).ToString("hh:mm:ss"));
                } else
                {
                    result.Add(value.ToString());
                }
            }
            return result.ToArray();
        }
    }
}
