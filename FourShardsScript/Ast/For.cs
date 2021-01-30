namespace FourShardsScript.Ast
{
    public class For : AstNode
    {
        public StatementList Init { get; }
        public AstNode Condition { get; }
        public StatementList Update { get; }
        public AstNode Body { get; }

        public For(StatementList init, AstNode condition, StatementList update, AstNode body)
        {
            Init = init;
            Condition = condition;
            Update = update;
            Body = body;
        }
    }
}