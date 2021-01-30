using System;

namespace FourShardsScript.Operators
{
    public static class OperatorPlus
    {
        public static object Plus(this object a, object b)
        {
            switch (a)
            {
                case int intValue:
                    return intValue.Plus(b);
                case double doubleValue:
                    return doubleValue.Plus(b);
            }

            throw new InvalidOperationException($"No override of operator '+' for arguments '{a}' and '{b}'");
        }

        public static object Plus(this int a, object b)
        {
            switch (b)
            {
                case int intValue:
                    return a.Plus(intValue);
                case double doubleValue:
                    return a.Plus(doubleValue);
            }

            throw new InvalidOperationException($"No override of operator '+' for arguments '{a}' and '{b}'");
        }

        public static object Plus(this double a, object b)
        {
            switch (b)
            {
                case int intValue:
                    return a.Plus(intValue);
                case double doubleValue:
                    return a.Plus(doubleValue);
            }

            throw new InvalidOperationException($"No override of operator '+' for arguments '{a}' and '{b}'");
        }

        public static object Plus(this int a, int b)
        {
            return a + b;
        }

        public static object Plus(this int a, double b)
        {
            return a + b;
        }

        public static object Plus(this double a, int b)
        {
            return a + b;
        }

        public static object Plus(this double a, double b)
        {
            return a + b;
        }
    }
}