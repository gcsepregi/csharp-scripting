using System.Collections.Generic;

namespace FourShardsScript.Ast
{
    public class Script : AstNode
    {
        public AstNode StatementList { get; }

        public Script(AstNode statementList)
        {
            StatementList = statementList;
        }
    }
}