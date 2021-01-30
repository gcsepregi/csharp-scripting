using System;

namespace FourShardsScript.Operators
{
    public static class OperatorGreaterEquals
    {
        public static bool GreaterEquals(this object a, object b)
        {
            if (a is int intValue)
            {
                return intValue.GreaterEquals(b);
            }
            else if (a is double doubleValue)
            {
                return doubleValue.GreaterEquals(b);
            }

            throw new InvalidOperationException("Invalid operand types");
        }

        public static bool GreaterEquals(this int a, object b)
        {
            if (b is int intValue)
            {
                return a.GreaterEquals(intValue);
            }
            else if (b is double doubleValue)
            {
                return a.GreaterEquals(doubleValue);
            }

            throw new InvalidOperationException("Invalid operand types");
        }

        public static bool GreaterEquals(this double a, object b)
        {
            if (b is int intValue)
            {
                return a.GreaterEquals(intValue);
            }
            else if (b is double doubleValue)
            {
                return a.GreaterEquals(doubleValue);
            }

            throw new InvalidOperationException("Invalid operand types");
        }

        public static bool GreaterEquals(this int a, int b)
        {
            return a >= b;
        }

        public static bool GreaterEquals(this int a, double b)
        {
            return a >= b;
        }

        public static bool GreaterEquals(this double a, int b)
        {
            return a >= b;
        }

        public static bool GreaterEquals(this double a, double b)
        {
            return a >= b;
        }

    }
}