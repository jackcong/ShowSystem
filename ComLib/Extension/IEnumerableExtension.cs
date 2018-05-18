using System;
using System.Collections.Generic;

namespace ComLib.Extension
{
    public static class IEnumerableExtension
    {
        public static void Each<T>(this IEnumerable<T> param, Action<T> action)
        {
            foreach(var c in param)
            {
                action(c);
            }
        }
    }
}
