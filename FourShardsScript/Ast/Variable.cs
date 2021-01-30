namespace FourShardsScript.Ast
{
    public class Variable : AstNode
    {
        public string Name { get; }
        
        public Variable(Token token)
        {
            Name = (string) token.Value;
        }
    }
}