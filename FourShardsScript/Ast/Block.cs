namespace FourShardsScript.Ast
{
    public class Block : AstNode
    {
        public AstNode StatementList { get; }

        public Block(AstNode statementList)
        {
            StatementList = statementList;
        }
    }
}