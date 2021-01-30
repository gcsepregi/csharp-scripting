using System;

namespace FourShardsScript.Operators
{
    public static class OperatorMul
    {
        public static object Mul(this object a, object b)
        {
            switch (a)
            {
                case int intValue:
                    return intValue.Mul(b);
                case double doubleValue:
                    return doubleValue.Mul(b);
            }

            throw new InvalidOperationException($"No override of operator '*' for arguments '{a}' and '{b}'");
        }

        public static object Mul(this int a, object b)
        {
            switch (b)
            {
                case int intValue:
                    return a.Mul(intValue);
                case double doubleValue:
                    return a.Mul(doubleValue);
            }

            throw new InvalidOperationException($"No override of operator '*' for arguments '{a}' and '{b}'");
        }

        public static object Mul(this double a, object b)
        {
            switch (b)
            {
                case int intValue:
                    return a.Mul(intValue);
                case double doubleValue:
                    return a.Mul(doubleValue);
            }

            throw new InvalidOperationException($"No override of operator '*' for arguments '{a}' and '{b}'");
        }

        public static object Mul(this int a, int b)
        {
            return a * b;
        }

        public static object Mul(this int a, double b)
        {
            return a * b;
        }

        public static object Mul(this double a, int b)
        {
            return a * b;
        }

        public static object Mul(this double a, double b)
        {
            return a * b;
        }
    }
}