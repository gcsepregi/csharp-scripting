namespace FourShardsScript.Ast
{
    public class Postfix : AstNode
    {
        public AstNode Left { get; }
        public TokenType Op { get; }

        public Postfix(AstNode left, Token op)
        {
            Left = left;
            Op = op.Type;
        }
    }
}