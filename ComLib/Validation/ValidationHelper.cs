using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ComLib.Validation
{
    public class ValidationHelper
    {
        public static bool IsNumber(string number)
        {
            if (Regex.IsMatch(number, @"^[-+]?\d*\.?\d*$") && !string.IsNullOrEmpty(number))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsNegativeNumber(string number)
        {
            if (Regex.IsMatch(number, @"^-\d*\.?\d*$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsPlus(string number)
        {
            if (Regex.IsMatch(number, @"^\d+(\.\d+)?$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsPlusIntegerNotZero(string number)
        {
            if (Regex.IsMatch(number, @"^\+?[1-9][0-9]*$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsPlusIntegerIncludeZero(string number)
        {
            return Regex.IsMatch(number, @"^0*[1-9]\d*$|0$");
        }

        public static bool IsMedRangePositiveDecimal(string number)
        {
            return Regex.IsMatch(number, @"^0*\d{0,7}(\.(\d{1,5}|0*))?$");
        }
    }
}
