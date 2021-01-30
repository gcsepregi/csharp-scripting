namespace FourShardsScript.Ast
{
    public class While : AstNode
    {
        public AstNode Condition { get; }
        public AstNode Body { get; }

        public While(AstNode condition, AstNode body)
        {
            Condition = condition;
            Body = body;
        }
    }
}