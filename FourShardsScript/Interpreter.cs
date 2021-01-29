using System;
using FourShardsScript.Ast;
using FourShardsScript.Visitor;

namespace FourShardsScript
{
    public class Interpreter : NodeVisitor
    {
        private readonly Parser _parser;

        public Interpreter(Parser parser)
        {
            _parser = parser;
        }

        public object VisitBinOp(BinOp node)
        {
            switch (node.Op.Type)
            {
                case TokenType.Plus:
                    return (int)Visit(node.Left) + (int)Visit(node.Right);
                case TokenType.Minus:
                    return (int)Visit(node.Left) - (int)Visit(node.Right);
                case TokenType.Mul:
                    return (int)Visit(node.Left) * (int)Visit(node.Right);
                case TokenType.Div:
                    return (int)Visit(node.Left) / (int)Visit(node.Right);
                default:
                    throw new Exception($"Expected operator types: Plus, Minus, Mul, Div. Got: {node.Op.Type}");
            }
        }

        public object VisitUnaryOp(UnaryOp node)
        {
            switch (node.Op.Type)
            {
                case TokenType.Plus:
                    return Visit(node.Expr);
                case TokenType.Minus:
                    return -(int) Visit(node.Expr);
                default:
                    throw new Exception($"Expected operators: Plus, Minus. Got: {node.Op.Type}");
            }
        }

        public object VisitNum(Num node)
        {
            return node.Value;
        }

        public object Interpret()
        {
            var tree = _parser.Parse();
            return Visit(tree);
        }
    }
}