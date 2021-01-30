using System;

namespace FourShardsScript.Operators
{
    public static class OperatorGreaterThan
    {
        public static bool GreaterThan(this object a, object b)
        {
            if (a is int intValue)
            {
                return intValue.GreaterThan(b);
            }
            else if (a is double doubleValue)
            {
                return doubleValue.GreaterThan(b);
            }

            throw new InvalidOperationException("Invalid operand types");
        }

        public static bool GreaterThan(this int a, object b)
        {
            if (b is int intValue)
            {
                return a.GreaterThan(intValue);
            }
            else if (b is double doubleValue)
            {
                return a.GreaterThan(doubleValue);
            }

            throw new InvalidOperationException("Invalid operand types");
        }

        public static bool GreaterThan(this double a, object b)
        {
            if (b is int intValue)
            {
                return a.GreaterThan(intValue);
            }
            else if (b is double doubleValue)
            {
                return a.GreaterThan(doubleValue);
            }

            throw new InvalidOperationException("Invalid operand types");
        }

        public static bool GreaterThan(this int a, int b)
        {
            return a > b;
        }

        public static bool GreaterThan(this int a, double b)
        {
            return a > b;
        }

        public static bool GreaterThan(this double a, int b)
        {
            return a > b;
        }

        public static bool GreaterThan(this double a, double b)
        {
            return a > b;
        }

    }
}