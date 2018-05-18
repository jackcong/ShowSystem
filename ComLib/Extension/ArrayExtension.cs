using System;

namespace ComLib.Extension
{
    public static class ArrayExtension
    {
        public static void Each<T>(this T[] param, Action<T> action)
        {
            foreach(var c in param)
            {
                action(c);
            }
        }
    }
}
