namespace FourShardsScript.Ast
{
    public class UnaryOp : IAstNode
    {
        public Token Op { get; }
        public IAstNode Expr { get; }

        public UnaryOp(Token op, IAstNode expr)
        {
            Op = op;
            Expr = expr;
        }
    }
}