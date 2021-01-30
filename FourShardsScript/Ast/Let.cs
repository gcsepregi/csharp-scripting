namespace FourShardsScript.Ast
{
    public class Let : AstNode
    {
        public AstNode Expr { get; }
        public string Name { get; }

        public Let(Token name, AstNode expr)
        {
            Name = (string)name.Value;
            Expr = expr;
        }

    }
}