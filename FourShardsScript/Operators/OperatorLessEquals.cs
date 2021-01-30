using System;

namespace FourShardsScript.Operators
{
    public static class OperatorLessEquals
    {
        public static bool LessEquals(this object a, object b)
        {
            if (a is int intValue)
            {
                return intValue.LessEquals(b);
            }
            else if (a is double doubleValue)
            {
                return doubleValue.LessEquals(b);
            }

            throw new InvalidOperationException("Invalid operand types");
        }

        public static bool LessEquals(this int a, object b)
        {
            if (b is int intValue)
            {
                return a.LessEquals(intValue);
            }
            else if (b is double doubleValue)
            {
                return a.LessEquals(doubleValue);
            }

            throw new InvalidOperationException("Invalid operand types");
        }

        public static bool LessEquals(this double a, object b)
        {
            if (b is int intValue)
            {
                return a.LessEquals(intValue);
            }
            else if (b is double doubleValue)
            {
                return a.LessEquals(doubleValue);
            }

            throw new InvalidOperationException("Invalid operand types");
        }

        public static bool LessEquals(this int a, int b)
        {
            return a <= b;
        }

        public static bool LessEquals(this int a, double b)
        {
            return a <= b;
        }

        public static bool LessEquals(this double a, int b)
        {
            return a <= b;
        }

        public static bool LessEquals(this double a, double b)
        {
            return a <= b;
        }

    }
}