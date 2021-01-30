namespace FourShardsScript.Ast
{
    public class Break : AstNode
    {
        public Token Token { get; }

        public Break(Token token)
        {
            Token = token;
        }
    }
}