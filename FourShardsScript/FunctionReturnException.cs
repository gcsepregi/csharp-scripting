using System;

namespace FourShardsScript
{
    public class FunctionReturnException : Exception
    {
        public object Value { get; }

        public FunctionReturnException(object value)
        {
            Value = value;
        }
    }
}