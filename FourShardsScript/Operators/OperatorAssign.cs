using FourShardsScript.Ast;

namespace FourShardsScript.Operators
{
    public static class OperatorAssign
    {

        public static object Assign(this AstNode left, object right, Context scope)
        {
            var l = (Variable) left;
            scope[l.Name] = right;
            return right;
        }
    }
}