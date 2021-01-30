namespace FourShardsScript.Ast
{
    public class BinOp : AstNode
    {
        public AstNode Left { get; }
        public Token Op { get; }
        public AstNode Right { get; }

        public BinOp(AstNode left, Token op, AstNode right)
        {
            Left = left;
            Op = op;
            Right = right;
        }
    }
}