namespace FourShardsScript.Ast
{
    public class Bool : AstNode
    {
        public bool Value { get; }
        public Bool(Token token)
        {
            Value = (bool) token.Value;
        }
    }
}