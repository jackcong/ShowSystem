using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;


namespace ComLib.Extension
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class StringValueAttribute : Attribute
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public StringValueAttribute()
        {
            Key = string.Empty;
        }

        public StringValueAttribute(string value):this()
        {
            Value = value;
        }

        public StringValueAttribute(string key, string value):this()
        {
            Key = key;
            Value = value;
        }
    }

    public static class EnumStringExtensions
    {

        // TODO: Cache this!
        public static string StringValue(this Enum par, string key = "")
        {
            Type type = par.GetType();
            string literalString = par.ToString();
            FieldInfo fieldInfo = type.GetField(literalString);
            StringValueAttribute[] attribs =
                fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

            var j = attribs.FirstOrDefault(c => c.Key == key);

            if (j != null)
                return j.Value;

            throw new KeyNotFoundException("No such key.");
        }
        public static string GetString<T>(this T value) //where T : struct
        {
            if (typeof(T).IsEnum)
            {
                FieldInfo field = typeof(T).GetField(value.ToString());
                if (field.IsDefined(typeof(DescriptionAttribute), false))
                {
                    DescriptionAttribute[] xmlEnum = (DescriptionAttribute[])typeof(T).GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                    return xmlEnum[0].Description;
                }
            }
            return value.ToString();
        }

    }
}
