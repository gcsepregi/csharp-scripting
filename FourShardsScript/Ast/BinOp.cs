namespace FourShardsScript.Ast
{
    public class BinOp : IAstNode
    {
        public IAstNode Left { get; }
        public Token Op { get; }
        public IAstNode Right { get; }

        public BinOp(IAstNode left, Token op, IAstNode right)
        {
            Left = left;
            Op = op;
            Right = right;
        }
    }
}