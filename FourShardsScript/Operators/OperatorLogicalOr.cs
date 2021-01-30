using System;

namespace FourShardsScript.Operators
{
    public static class OperatorLogicalOr
    {

        public static object LogicalOr(this object a, object b)
        {
            if (a is bool b1 && b is bool b2)
            {
                return b1 || b2;
            }

            throw new InvalidOperationException("Invalid operand types for &&");
        }
    }
}