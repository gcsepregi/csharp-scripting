using System;
using FourShardsScript.Ast;

namespace FourShardsScript.Visitor
{
    public class NodeVisitor
    {
        public object Visit(AstNode node)
        {
            var type = this.GetType();
            var method = type.GetMethod($"Visit{node.GetType().Name}");
            if (method == null)
            {
                throw new Exception($"Method not found: Visit{node.GetType().Name}");
            }

            try
            {
                return method.Invoke(this, new object[] {node});
            }
            catch (Exception e)
            {
                if (e.InnerException != null) throw e.InnerException;
                throw;
            }
        }
    }
}