using System;

namespace ComLib.Extension
{
    public static class TypeExtension
    {
        public static Type GetUnderlyingType(this Type param)
        {
            if (param.IsValueType)
            {
                if (param.IsGenericType && param.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return Nullable.GetUnderlyingType(param);
                }
                else
                {
                    return param;
                }

            } else
            {
                return param;
            }
        }
    }
}
