using System;

namespace FourShardsScript.Operators
{
    public static class OperatorLessThan
    {
        public static bool LessThan(this object a, object b)
        {
            if (a is int intValue)
            {
                return intValue.LessThan(b);
            }
            else if (a is double doubleValue)
            {
                return doubleValue.LessThan(b);
            }

            throw new InvalidOperationException("Invalid operand types");
        }

        public static bool LessThan(this int a, object b)
        {
            if (b is int intValue)
            {
                return a.LessThan(intValue);
            }
            else if (b is double doubleValue)
            {
                return a.LessThan(doubleValue);
            }

            throw new InvalidOperationException("Invalid operand types");
        }

        public static bool LessThan(this double a, object b)
        {
            if (b is int intValue)
            {
                return a.LessThan(intValue);
            }
            else if (b is double doubleValue)
            {
                return a.LessThan(doubleValue);
            }

            throw new InvalidOperationException("Invalid operand types");
        }

        public static bool LessThan(this int a, int b)
        {
            return a < b;
        }

        public static bool LessThan(this int a, double b)
        {
            return a < b;
        }

        public static bool LessThan(this double a, int b)
        {
            return a < b;
        }

        public static bool LessThan(this double a, double b)
        {
            return a < b;
        }

    }
}