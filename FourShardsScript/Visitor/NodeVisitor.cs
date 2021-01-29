using FourShardsScript.Ast;

namespace FourShardsScript.Visitor
{
    public class NodeVisitor
    {
        public object Visit(IAstNode node)
        {
            var type = this.GetType();
            var method = type.GetMethod($"Visit{node.GetType().Name}");
            return method?.Invoke(this, new object[] {node});
        }
    }
}