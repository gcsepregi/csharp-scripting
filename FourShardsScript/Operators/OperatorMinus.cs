using System;

namespace FourShardsScript.Operators
{
    public static class OperatorMinus
    {
        public static object Minus(this object a, object b = null)
        {
            switch (a)
            {
                case int intValue:
                    return intValue.Minus(b);
                case double doubleValue:
                    return doubleValue.Minus(b);
            }

            throw new InvalidOperationException($"No override of operator '-' for arguments '{a}' and '{b}'");
        }

        public static object Minus(this int a, object b)
        {
            if (b == null)
            {
                return -a;
            }

            switch (b)
            {
                case int intValue:
                    return a.Minus(intValue);
                case double doubleValue:
                    return a.Minus(doubleValue);
            }

            throw new InvalidOperationException($"No override of operator '-' for arguments '{a}' and '{b}'");
        }

        public static object Minus(this double a, object b)
        {
            switch (b)
            {
                case int intValue:
                    return a.Minus(intValue);
                case double doubleValue:
                    return a.Minus(doubleValue);
            }

            throw new InvalidOperationException($"No override of operator '-' for arguments '{a}' and '{b}'");
        }

        public static object Minus(this int a, int b)
        {
            return a - b;
        }

        public static object Minus(this int a, double b)
        {
            return a - b;
        }

        public static object Minus(this double a, int b)
        {
            return a - b;
        }

        public static object Minus(this double a, double b)
        {
            return a - b;
        }
    }
}