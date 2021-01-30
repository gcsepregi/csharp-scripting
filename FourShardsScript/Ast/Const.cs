namespace FourShardsScript.Ast
{
    public class Const : AstNode
    {
        public AstNode Expr { get; }
        public string Name { get; }

        public Const(Token name, AstNode expr)
        {
            Name = (string)name.Value;
            Expr = expr;
        }

    }
}