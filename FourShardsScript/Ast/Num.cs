namespace FourShardsScript.Ast
{
    public class Num : IAstNode
    {
        private Token _token;
        public object Value { get; }

        public Num(Token token)
        {
            _token = token;
            Value = token.Value;
        }
    }
}