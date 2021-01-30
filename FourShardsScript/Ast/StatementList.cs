using System.Collections.Generic;

namespace FourShardsScript.Ast
{
    public class StatementList : AstNode
    {
        public List<AstNode> Statements { get; }

        public StatementList(List<AstNode> statements)
        {
            Statements = statements;
        }
    }
}