namespace FourShardsScript.Ast
{
    public class UnaryOp : AstNode
    {
        public Token Op { get; }
        public AstNode Expr { get; }

        public UnaryOp(Token op, AstNode expr)
        {
            Op = op;
            Expr = expr;
        }
    }
}