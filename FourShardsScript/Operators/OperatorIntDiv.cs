using System;

namespace FourShardsScript.Operators
{
    public static class OperatorMod
    {
        public static object Mod(this object a, object b)
        {
            switch (a)
            {
                case int intValue:
                    return intValue.Mod(b);
                case double doubleValue:
                    return doubleValue.Mod(b);
            }

            throw new InvalidOperationException($"No override of operator '/' for arguments '{a}' and '{b}'");
        }

        public static object Mod(this int a, object b)
        {
            switch (b)
            {
                case int intValue:
                    return a.Mod(intValue);
                case double doubleValue:
                    return a.Mod(doubleValue);
            }

            throw new InvalidOperationException($"No override of operator '/' for arguments '{a}' and '{b}'");
        }

        public static object Mod(this double a, object b)
        {
            switch (b)
            {
                case int intValue:
                    return a.Mod(intValue);
                case double doubleValue:
                    return a.Mod(doubleValue);
            }

            throw new InvalidOperationException($"No override of operator '/' for arguments '{a}' and '{b}'");
        }

        public static object Mod(this int a, int b)
        {
            return a % b;
        }

        public static object Mod(this int a, double b)
        {
            return a % b;
        }

        public static object Mod(this double a, int b)
        {
            return a % b;
        }

        public static object Mod(this double a, double b)
        {
            return a % b;
        }
    }
}