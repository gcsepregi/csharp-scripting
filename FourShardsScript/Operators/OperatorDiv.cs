using System;

namespace FourShardsScript.Operators
{
    public static class OperatorDiv
    {
        public static object Div(this object a, object b)
        {
            switch (a)
            {
                case int intValue:
                    return intValue.Div(b);
                case double doubleValue:
                    return doubleValue.Div(b);
            }

            throw new InvalidOperationException($"No override of operator '/' for arguments '{a}' and '{b}'");
        }

        public static object Div(this int a, object b)
        {
            switch (b)
            {
                case int intValue:
                    return a.Div(intValue);
                case double doubleValue:
                    return a.Div(doubleValue);
            }

            throw new InvalidOperationException($"No override of operator '/' for arguments '{a}' and '{b}'");
        }

        public static object Div(this double a, object b)
        {
            switch (b)
            {
                case int intValue:
                    return a.Div(intValue);
                case double doubleValue:
                    return a.Div(doubleValue);
            }

            throw new InvalidOperationException($"No override of operator '/' for arguments '{a}' and '{b}'");
        }

        public static object Div(this int a, int b)
        {
            return a / b;
        }

        public static object Div(this int a, double b)
        {
            return a / b;
        }

        public static object Div(this double a, int b)
        {
            return a / b;
        }

        public static object Div(this double a, double b)
        {
            return a / b;
        }
    }
}