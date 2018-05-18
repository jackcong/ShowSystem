using System;

namespace ComLib.Converter
{
    public static class NumberConverter
    {
        /// <summary>
        /// Converts the object to Int32.
        /// </summary>
        /// <typeparam name="T">Type of the object to convert.</typeparam>
        /// <param name="param">The object to convert.</param>
        /// <returns>The converted Int32 value.</returns>
        public static int ToInt32<T>(this T param)
        {
            return Convert.ToInt32(param);
        }

        /// <summary>
        /// Converts the object to Int32 despite any possible exception (Returns null on error).
        /// TODO: Low efficiency. Needs optimization.
        /// </summary>
        /// <typeparam name="T">Type of the object to convert.</typeparam>
        /// <param name="param">The object to convert.</param>
        /// <returns>The converted Int32 value.</returns>
        public static int? ToInt32Silent<T>(this T param)
        {
            try
            {
                return Convert.ToInt32(param.ToString());
            }
            catch
            {
                return null;
            }
        }

        public static int? ToInt32Loose(this string param)
        {
            if(string.IsNullOrWhiteSpace(param))
            {
                return null;
            } else
            {
                return param.ToInt32();
            }
        }

        /// <summary>
        /// Converts the object to Int64.
        /// </summary>
        /// <typeparam name="T">Type of the object to convert.</typeparam>
        /// <param name="param">The object to convert.</param>
        /// <returns>The converted Int64 value.</returns>
        public static long ToInt64<T>(this T param)
        {
            return Convert.ToInt64(param);
        }

        /// <summary>
        /// Converts the object to Int64 despite any possible exception (Returns null on error).
        /// TODO: Low efficiency. Needs optimization.
        /// </summary>
        /// <typeparam name="T">Type of the object to convert.</typeparam>
        /// <param name="param">The object to convert.</param>
        /// <returns>The converted Int64 value.</returns>
        public static long? ToInt64Silent<T>(this T param)
        {
            try
            {
                return Convert.ToInt64(param.ToString());
            }
            catch
            {
                return null;
            }
        }

        public static long? ToInt64Loose(this string param)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                return null;
            }
            else
            {
                return param.ToInt64();
            }
        }

        /// <summary>
        /// Converts the object to double.
        /// </summary>
        /// <typeparam name="T">Type of the object to convert.</typeparam>
        /// <param name="param">The object to convert.</param>
        /// <returns>The converted double value.</returns>
        public static double ToDouble<T>(this T param)
        {
            return Convert.ToDouble(param);
        }

        /// <summary>
        /// Converts the object to double despite any possible exception (Returns null on error).
        /// TODO: Low efficiency. Needs optimization.
        /// </summary>
        /// <typeparam name="T">Type of the object to convert.</typeparam>
        /// <param name="param">The object to convert.</param>
        /// <returns>The converted double value.</returns>
        public static double? ToDoubleSilent<T>(this T param)
        {
            try
            {
                return Convert.ToDouble(param.ToString());
            }
            catch
            {
                return null;
            }
        }

        public static double? ToDoubleLoose(this string param)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                return null;
            }
            else
            {
                return param.ToDouble();
            }
        }

        /// <summary>
        /// Converts the object to decimal.
        /// </summary>
        /// <typeparam name="T">Type of the object to convert.</typeparam>
        /// <param name="param">The object to convert.</param>
        /// <returns>The converted decimal value.</returns>
        public static decimal ToDecimal<T>(this T param)
        {
            return Convert.ToDecimal(param);
        }

        /// <summary>
        /// Converts the object to decimal despite any possible exception (Returns null on error).
        /// TODO: Low efficiency. Needs optimization.
        /// </summary>
        /// <typeparam name="T">Type of the object to convert.</typeparam>
        /// <param name="param">The object to convert.</param>
        /// <returns>The converted decimal value.</returns>
        public static decimal? ToDecimalSilent<T>(this T param)
        {
            try
            {
                return Convert.ToDecimal(param.ToString());
            }
            catch
            {
                return null;
            }
        }

        public static decimal? ToDecimalLoose(this string param)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                return null;
            }
            else
            {
                return param.ToDecimal();
            }
        }

    }
}
