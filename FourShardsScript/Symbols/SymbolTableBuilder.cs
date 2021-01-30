using System;
using System.Collections.Generic;
using FourShardsScript.Ast;
using FourShardsScript.Visitor;

namespace FourShardsScript.Symbols
{
    public class SymbolTableBuilder : NodeVisitor
    {

        private SymbolTable _symbols;

        private readonly Stack<SymbolTable> _stack = new Stack<SymbolTable>();

        public void VisitScript(Script node)
        {
            EnterScope(node);
            Visit(node.StatementList);
            ExitScope();
        }

        public void VisitStatementList(StatementList node)
        {
            foreach (var statement in node.Statements)
            {
                Visit(statement);
            }
        }

        public void VisitNum(Num node)
        {
        }

        public void VisitBinOp(BinOp node)
        {
            if (node.Op.Type == TokenType.Assign)
            {
                var symbol = _symbols.Lookup(((Variable) node.Left).Name, true);
                if (symbol == null)
                {
                    throw new Exception($"Variable '{((Variable)node.Left).Name}' haven't been declared");
                }
                if (!(symbol is LetSymbol))
                {
                    throw new Exception($"Cannot assign value to symbol of type {symbol.GetType()}");
                }
            }
            else
            {
                Visit(node.Left);
            }

            Visit(node.Right);
        }

        public void VisitUnaryOp(UnaryOp node)
        {
            Visit(node.Expr);
        }

        public void VisitLet(Let node)
        {
            _symbols.Define(new LetSymbol(node.Name));
            Visit(node.Expr);
        }

        public void VisitConst(Const node)
        {
            _symbols.Define(new ConstSymbol(node.Name));
            Visit(node.Expr);
        }

        public void VisitIf(If node)
        {
            Visit(node.Condition);
            Visit(node.TrueStatement);
            Visit(node.ElseStatement);
        }

        public void VisitVariable(Variable node)
        {
            try
            {
                if (_symbols.Lookup(node.Name, true) == null)
                {
                    throw new Exception($"Variable must be declared before first use: {node.Name}");
                }
            }
            catch (KeyNotFoundException)
            {
                throw new Exception($"Variable must be declared before first use: {node.Name}");
            }
        }

        public void VisitBool(Bool node)
        {
        }

        public void VisitBlock(Block node)
        {
            EnterScope(node);
            Visit(node.StatementList);
            ExitScope();
        }

        public void VisitFor(For node)
        {
            EnterScope(node);
            Visit(node.Init);
            Visit(node.Condition);
            Visit(node.Body);
            Visit(node.Update);
            ExitScope();
        }

        public void VisitWhile(While node)
        {
            Visit(node.Condition);
            Visit(node.Body);
        }

        public void VisitFunctionDecl(FunctionDecl node)
        {
            _symbols.Define(new FunctionSymbol(node.Name, node));
            EnterScope(node);
            foreach (var arg in node.Args)
            {
                _symbols.Define(new FormalArgumentSymbol(arg));
            }
            Visit(node.Body);
            ExitScope();
        }

        public void VisitReturn(Return node)
        {
            Visit(node.Expr);
        }

        public void VisitFunctionCall(FunctionCall node)
        {
            foreach (var arg in node.Args)
            {
                Visit(arg);
            }
        }

        public void VisitNoOp(NoOp node)
        {
        }

        public void VisitContinue(Continue node)
        {
        }

        public void VisitBreak(Break node)
        {
        }

        private void EnterScope(AstNode node)
        {
            _stack.Push(_symbols);
            _symbols = new SymbolTable(_symbols);
            node.Annotate(new SymbolTableAnnotation(_symbols));
        }

        private void ExitScope()
        {
            _symbols = _stack.Pop();
        }
    }
}