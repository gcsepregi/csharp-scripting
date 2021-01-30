using FourShardsScript.Ast;

namespace FourShardsScript.Symbols
{
    public class FunctionSymbol : Symbol
    {
        public AstNode FunctionDecl { get; }

        public FunctionSymbol(string name, AstNode functionDecl) : base(name)
        {
            FunctionDecl = functionDecl;
        }
    }
}