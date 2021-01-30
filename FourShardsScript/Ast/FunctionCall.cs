using System.Collections.Generic;

namespace FourShardsScript.Ast
{
    public class FunctionCall : AstNode
    {
        public List<AstNode> Args { get; }
        public string Name { get; }
     
        public FunctionCall(Token name, List<AstNode> args)
        {
            Args = args;
            Name = (string)name.Value;
        }
    }
}