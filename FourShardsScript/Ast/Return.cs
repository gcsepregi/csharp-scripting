namespace FourShardsScript.Ast
{
    public class Return : AstNode
    {
        public AstNode Expr { get; }

        public Return(AstNode expr)
        {
            Expr = expr;
        }
    }
}