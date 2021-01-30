namespace FourShardsScript.Ast
{
    public class If : AstNode
    {
        public AstNode Condition { get; }
        public AstNode TrueStatement { get; }
        public AstNode ElseStatement { get; }

        public If(AstNode condition, AstNode trueStatement, AstNode elseStatement)
        {
            Condition = condition;
            TrueStatement = trueStatement;
            ElseStatement = elseStatement;
        }
    }
}