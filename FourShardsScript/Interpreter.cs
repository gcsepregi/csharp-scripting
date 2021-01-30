using System;
using System.Collections.Generic;
using System.Linq;
using FourShardsScript.Ast;
using FourShardsScript.Operators;
using FourShardsScript.Symbols;
using FourShardsScript.Visitor;

namespace FourShardsScript
{
    public class Interpreter : NodeVisitor
    {
        private readonly AstNode _tree;
        private Context _currentContext;
        private readonly Stack<Context> _contexts = new Stack<Context>();

        public Interpreter(AstNode tree)
        {
            _tree = tree;
        }

        public object Interpret()
        {
            return Visit(_tree);
        }
        
        public object VisitScript(Script node)
        {
            EnterScope(node);
            var result = Visit(node.StatementList);
            ExitScope();
            return result;
        }

        public object VisitStatementList(StatementList node)
        {
            object result = null;
            node.Statements.ForEach(stmt =>
            {
                result = Visit(stmt);
            });
            return result;
        }

        public object VisitConst(Const node)
        {
            var result = Visit(node.Expr);
            _currentContext.SetConstantValue(node.Name, result);
            return result;
        }
        
        public object VisitLet(Let node)
        {
            var result = Visit(node.Expr);
            _currentContext[node.Name] = result;
            return result;
        }

        public object VisitVariable(Variable variable)
        {
            return _currentContext[variable.Name];
        }

        public object VisitIf(If node)
        {
            var condition = Visit(node.Condition);
            if ((bool) condition)
            {
                return Visit(node.TrueStatement);
            }

            return Visit(node.ElseStatement);
        }

        public object VisitNoOp(NoOp node)
        {
            return null;
        }

        public object VisitBinOp(BinOp node)
        {
            switch (node.Op.Type)
            {
                case TokenType.Plus:
                    return Visit(node.Left).Plus(Visit(node.Right));
                case TokenType.Minus:
                    return Visit(node.Left).Minus(Visit(node.Right));
                case TokenType.Mul:
                    return Visit(node.Left).Mul(Visit(node.Right));
                case TokenType.Div:
                    return Visit(node.Left).Div(Visit(node.Right));
                case TokenType.Remainder:
                    return Visit(node.Left).Mod(Visit(node.Right));
                case TokenType.Assign:
                    return node.Left.Assign(Visit(node.Right), _currentContext);
                case TokenType.LogicalAnd:
                    return Visit(node.Left).LogicalAnd(Visit(node.Right));
                case TokenType.LogicalOr:
                    return Visit(node.Left).LogicalOr(Visit(node.Right));
                case TokenType.Equals_:
                    return Visit(node.Left).Equals(Visit(node.Right));
                case TokenType.NotEquals:
                    return !Visit(node.Left).Equals(Visit(node.Right));
                case TokenType.Less:
                    return Visit(node.Left).LessThan(Visit(node.Right));
                case TokenType.LessOrEquals:
                    return Visit(node.Left).LessEquals(Visit(node.Right));
                case TokenType.Greater:
                    return Visit(node.Left).GreaterThan(Visit(node.Right));
                case TokenType.GreaterOrEqual:
                    return Visit(node.Left).GreaterEquals(Visit(node.Right));
                default:
                    throw new Exception($"Unknown operator {node.Op.Type}");
            }
        }

        public object VisitUnaryOp(UnaryOp node)
        {
            switch (node.Op.Type)
            {
                case TokenType.Plus:
                    return Visit(node.Expr);
                case TokenType.Minus:
                    return Visit(node.Expr).Minus();
                case TokenType.Negate:
                    return !(bool) Visit(node.Expr);
                default:
                    throw new Exception($"Unknown operator {node.Op}");
            }
        }
        
        public object VisitFor(For node)
        {
            EnterScope(node);
            Visit(node.Init);
            object result = null;
            while ((bool) Visit(node.Condition))
            {
                try
                {
                    result = Visit(node.Body);
                }
                catch (LoopBreakException)
                {
                    break;
                }
                catch (LoopContinueException)
                {
                }

                Visit(node.Update);
            }
            ExitScope();
            return result;
        }

        public object VisitWhile(While node)
        {
            object result = null;

            while ((bool) Visit(node.Condition))
            {
                try
                {
                    result = Visit(node.Body);
                }
                catch (LoopBreakException)
                {
                    break;
                }
                catch (LoopContinueException)
                {
                }
            }
            
            return result;
        }

        public object VisitFunctionDecl(FunctionDecl node)
        {
            return null;
        }

        public object VisitFunctionCall(FunctionCall node)
        {
            var functionDecl = (FunctionDecl)_currentContext[node.Name];
            EnterScope(functionDecl);
            foreach (var (formal, actual) in functionDecl.Args.Zip(node.Args))
            {
                _currentContext[formal] = Visit(actual);
            }

            object result = null;
            try
            {
                VisitBlock((Block) functionDecl.Body);
            }
            catch (FunctionReturnException e)
            {
                result = e.Value;
            }

            ExitScope();
            return result;
        }
        
        public object VisitReturn(Return node)
        {
            throw new FunctionReturnException(Visit(node.Expr));
        }
        
        public object VisitContinue(Continue node)
        {
            throw new LoopContinueException();
        }

        public object VisitBreak(Break node)
        {
            throw new LoopBreakException();
        }

        public object VisitBlock(Block node)
        {
            EnterScope(node);
            var result = Visit(node.StatementList);
            ExitScope();
            return result;
        }

        public object VisitNum(Num node)
        {
            return node.Value;
        }

        public object VisitBool(Bool node)
        {
            return node.Value;
        }

        private void EnterScope(AstNode node)
        {
            var symbols = node.Annotation<SymbolTableAnnotation>();
            var newContext = new Context(symbols.Symbols, _currentContext);
            _contexts.Push(_currentContext);
            _currentContext = newContext;
        }

        private void ExitScope()
        {
            _currentContext = _contexts.Pop();
        }
    }
}