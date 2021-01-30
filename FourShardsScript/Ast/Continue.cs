namespace FourShardsScript.Ast
{
    public class Continue : AstNode
    {
        public Token Token { get; }

        public Continue(Token token)
        {
            Token = token;
        }
    }
}