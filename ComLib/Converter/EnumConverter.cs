using ComLib.Extension;
using System;
using System.Linq;
using System.Reflection;

namespace ComLib.Converter
{
    public static class EnumConverter
    {
        /// <summary>
        /// Converts a string to an enum value whose name is exactly the same as the given string.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="param">The string to convert.</param>
        /// <param name="ignoreCase">Whether the conversion is non-case-sensitive.</param>
        /// <returns>The converted enum value.</returns>
        public static T ToEnum<T>(this string param, bool ignoreCase=false)
        {
            return (T) Enum.Parse(typeof (T), param, ignoreCase);
        }

        /// <summary>
        /// Converts a string to an enum value whose value of StringValue attribute (if any) is the same as the given string. 
        /// If no satisfied StringValue attribute is found, an enum value whose name is the same as the given string is returned.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="param">The string to be converted.</param>
        /// <param name="key">The key.</param>
        /// <param name="ignoreCase">Whether the conversion is non-case-sensitive.</param>
        /// <returns></returns>
        public static T ToEnumByStringValue<T>(this string param, string key="", bool ignoreCase=false)
        {
            Type type = typeof (T);
            FieldInfo[] fieldInfos = type.GetFields();
            
            // TODO: Improve performance

            if(ignoreCase)
            {
                foreach (var fieldInfo in fieldInfos)
                {
                    StringValueAttribute[] attribs =
                fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                    if (attribs.Length == 0)
                        continue;
                    if (attribs.Any(c => c.Key == key && c.Value.ToLower() == param.ToLower()))
                    {
                        return (T)Enum.Parse(type, fieldInfo.Name, true);
                    }
                }
            } else
            {
                foreach (var fieldInfo in fieldInfos)
                {
                    StringValueAttribute[] attribs =
                fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                    if (attribs.Length == 0)
                        continue;
                    if (attribs.Any(c => c.Key == key && c.Value == param))
                    {
                        return (T)Enum.Parse(type, fieldInfo.Name, true);
                    }
                }
            }
            // TODO: Create a new exception class.
            throw new System.Exception("Enum conversion failed.");
        }
    }
}
