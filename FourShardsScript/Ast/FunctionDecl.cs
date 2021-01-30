using System.Collections.Generic;
using System.Linq;

namespace FourShardsScript.Ast
{
    public class FunctionDecl : AstNode
    {
        public IEnumerable<string> Args { get; }
        public AstNode Body { get; }
        public string Name { get; }
        
        public FunctionDecl(Token name, IEnumerable<Token> args, AstNode body)
        {
            Args = args.Select(a => (string)a.Value);
            Body = body;
            Name = (string)name.Value;
        }

    }
}