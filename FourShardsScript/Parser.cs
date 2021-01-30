using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FourShardsScript.Ast;

namespace FourShardsScript
{
    public class Parser
    {
        private readonly Lexer _lexer;
        private Token _currentToken;

        public Parser(Lexer lexer)
        {
            _lexer = lexer;
            _currentToken = _lexer.GetNextToken();
        }

        public AstNode Parse()
        {
            return new Script(StatementList());
        }

        private AstNode StatementList()
        {
            var result = new List<AstNode>();
            while (_currentToken.Type != TokenType.Eof && _currentToken.Type != TokenType.Rcparen)
            {
                result.Add(Statement());
            }

            return new StatementList(result);

        }

        private AstNode Statement()
        {
            switch (_currentToken.Type)
            {
                case TokenType.Let:
                    return Let();
                case TokenType.Const:
                    return Const();
                case TokenType.If:
                    return If();
                case TokenType.For:
                    return For();
                case TokenType.While:
                    return While();
                case TokenType.FunctionDecl:
                    return FunctionDecl();
                case TokenType.Return:
                    return Return();
                case TokenType.Lcparen:
                    return Block();
                default:
                    if (_currentToken.Type == TokenType.Id && _lexer.Peek().Type == TokenType.Lparen)
                    {
                        return FunctionCall();
                    }
                    else
                    {
                        var expr = Expr();
                        if (_currentToken.Type == TokenType.Semicolon)
                        {
                            Eat(TokenType.Semicolon);
                        }

                        return expr;
                    }
            }
        }
        
        private AstNode Let(TokenType statementSeparator = TokenType.Semicolon, bool optional = false)
        {
            Eat(TokenType.Let);
            var name = _currentToken;
            Eat(TokenType.Id);
            Eat(TokenType.Assign);
            var expr = Expr();
            Eat(statementSeparator, optional);
            return new Let(name, expr);
        }

        private AstNode Const()
        {
            Eat(TokenType.Const);
            var name = _currentToken;
            Eat(TokenType.Id);
            Eat(TokenType.Assign);
            var expr = Expr();
            Eat(TokenType.Semicolon);
            return new Const(name, expr);
        }

        private AstNode If()
        {
            Eat(TokenType.If);
            Eat(TokenType.Lparen);
            var condition = Expr();
            Eat(TokenType.Rparen);
            var trueStatement = Statement();
            if (_currentToken.Type != TokenType.Else) return new If(condition, trueStatement, new NoOp());
            Eat(TokenType.Else);
            return new If(condition, trueStatement, Statement());
        }

        public AstNode For()
        {
            Eat(TokenType.For);
            Eat(TokenType.Lparen);
            var init = new List<AstNode>();
            while (_currentToken.Type == TokenType.Let)
            {
                init.Add(Let(TokenType.Comma, true));
            }

            Eat(TokenType.Semicolon);
            var condition = _currentToken.Type != TokenType.Semicolon ? Expr() : new NoOp();
            Eat(TokenType.Semicolon);
            var update = new List<AstNode>();
            while (_currentToken.Type != TokenType.Rparen)
            {
                update.Add(Expr());
                if (_currentToken.Type != TokenType.Rparen)
                {
                    Eat(TokenType.Comma);
                }
            }

            Eat(TokenType.Rparen);
            var body = Statement();
            return new For(new StatementList(init), condition, new StatementList(update), body);
        }

        private AstNode While()
        {
            Eat(TokenType.While);
            Eat(TokenType.Lparen);
            var condition = Expr();
            Eat(TokenType.Rparen);
            var body = Statement();
            return new While(condition, body);
        }

        private AstNode FunctionDecl()
        {
            Eat(TokenType.FunctionDecl);
            var name = _currentToken;
            Eat(TokenType.Id);
            Eat(TokenType.Lparen);
            var args = new List<Token>();
            while (_currentToken.Type != TokenType.Rparen)
            {
                args.Add(_currentToken);
                Eat(TokenType.Id);
                if (_currentToken.Type == TokenType.Comma)
                {
                    Eat(TokenType.Comma);
                }
            }
            Eat(TokenType.Rparen);
            var body = Block();
            return new FunctionDecl(name, args, body);
        }

        private AstNode Return()
        {
            Eat(TokenType.Return);
            if (_currentToken.Type != TokenType.Semicolon)
            {
                var expr = Expr();
                Eat(TokenType.Semicolon);
                return new Return(expr);
            }
            Eat(TokenType.Semicolon);
            return new Return(new NoOp());
        }

        private AstNode FunctionCall()
        {
            var name = _currentToken;
            Eat(TokenType.Id);
            Eat(TokenType.Lparen);
            var args = new List<AstNode>();
            while (_currentToken.Type != TokenType.Rparen)
            {
                args.Add(Expr());
                if (_currentToken.Type == TokenType.Comma)
                {
                    Eat(TokenType.Comma);
                }
            }
            Eat(TokenType.Rparen);
            Eat(TokenType.Semicolon);
            return new FunctionCall(name, args);
        }
        
        private AstNode Block()
        {
            Eat(TokenType.Lcparen);
            var result = StatementList();
            Eat(TokenType.Rcparen);
            return new Block(result);
        }

        private AstNode Expr()
        {
            return _lexer.Peek().Type == TokenType.Assign ? Assignment() : Ternary();
        }
        
        private AstNode Assignment()
        {
            var left = Var();
            var op = _currentToken;
            Eat(TokenType.Assign);
            var right = Ternary();
            switch ((string) op.Value)
            {
                case "+=":
                    return new BinOp(left, op, new BinOp(left, new Token(TokenType.Plus, '+'), right));
                case "-=":
                    return new BinOp(left, op, new BinOp(left, new Token(TokenType.Minus, '-'), right));
                case "*=":
                    return new BinOp(left, op, new BinOp(left, new Token(TokenType.Mul, '*'), right));
                case "**=":
                    return new BinOp(left, op, new BinOp(left, new Token(TokenType.Exponential, '*'), right));
                case "%=":
                    return new BinOp(left, op, new BinOp(left, new Token(TokenType.Remainder, '%'), right));
                case "/=":
                    return new BinOp(left, op, new BinOp(left, new Token(TokenType.Div, '/'), right));
                case "&&=":
                    return new BinOp(left, op, new BinOp(left, new Token(TokenType.LogicalAnd, "&&"), right));
                case "||=":
                    return new BinOp(left, op, new BinOp(left, new Token(TokenType.LogicalOr, "||"), right));
                case "&=":
                    return new BinOp(left, op, new BinOp(left, new Token(TokenType.BitwiseAnd, "&"), right));
                case "|=":
                    return new BinOp(left, op, new BinOp(left, new Token(TokenType.BitwiseOr, "|"), right));
                case "ˆ=":
                    return new BinOp(left, op, new BinOp(left, new Token(TokenType.BitwiseOr, "ˆ"), right));
                case "<<=":
                    return new BinOp(left, op, new BinOp(left, new Token(TokenType.ShiftLeft, "<<"), right));
                case ">>=":
                    return new BinOp(left, op, new BinOp(left, new Token(TokenType.ShiftRight, ">>"), right));
                case ">>>=":
                    return new BinOp(left, op, new BinOp(left, new Token(TokenType.UnsignedShiftRight, ">>>"), right));
                default:
                    return new BinOp(left, op, right);
            }
        }

        private AstNode Ternary()
        {
            var condition = LogicalOr();
            if (_currentToken.Type != TokenType.QuestionMark)
            {
                return condition;
            }
            Eat(TokenType.QuestionMark);
            var trueExpression = LogicalOr();
            Eat(TokenType.Colon);
            var falseExpression = LogicalOr();
            return new If(condition, trueExpression, falseExpression);
        }

        private AstNode LogicalOr()
        {
            var left = LogicalAnd();
            while (_currentToken.Type == TokenType.LogicalOr)
            {
                var op = _currentToken;
                Eat(TokenType.LogicalOr);
                var right = LogicalAnd();
                left = new BinOp(left, op, right);
            }

            return left;
        }

        private AstNode LogicalAnd()
        {
            var left = BitwiseOr();
            while (_currentToken.Type == TokenType.LogicalAnd)
            {
                var op = _currentToken;
                Eat(TokenType.LogicalAnd);
                var right = BitwiseOr();
                left = new BinOp(left, op, right);
            }

            return left;
        }

        private AstNode BitwiseOr()
        {
            var left = BitwiseXor();
            while (_currentToken.Type == TokenType.BitwiseOr)
            {
                var op = _currentToken;
                Eat(TokenType.BitwiseOr);
                var right = BitwiseXor();
                left = new BinOp(left, op, right);
            }

            return left;
        }

        private AstNode BitwiseXor()
        {
            var left = BitwiseAnd();
            while (_currentToken.Type == TokenType.BitwiseXor)
            {
                var op = _currentToken;
                Eat(TokenType.BitwiseXor);
                var right = BitwiseAnd();
                left = new BinOp(left, op, right);
            }

            return left;
        }

        private AstNode BitwiseAnd()
        {
            var left = Equality();
            while (_currentToken.Type == TokenType.BitwiseAnd)
            {
                var op = _currentToken;
                Eat(TokenType.BitwiseAnd);
                var right = Equality();
                left = new BinOp(left, op, right);
            }

            return left;
        }

        private AstNode Equality()
        {
            var left = Relational();
            if (_currentToken.Type == TokenType.Equals_ || _currentToken.Type == TokenType.NotEquals)
            {
                var op = _currentToken;
                Eat(op.Type);
                return new BinOp(left, op, Relational());
            }

            return left;
        }

        private AstNode Relational()
        {
            var left = Shift();
            if (new [] {TokenType.Less, TokenType.LessOrEquals, TokenType.Greater, TokenType.GreaterOrEqual, TokenType.Instanceof}.Contains(_currentToken.Type))
            {
                var op = _currentToken;
                Eat(op.Type);
                return new BinOp(left, op, Shift());
            }

            return left;
        }

        private AstNode Shift()
        {
            var left = Additive();
            if (new[] {TokenType.ShiftLeft, TokenType.ShiftRight, TokenType.UnsignedShiftRight}.Contains(
                _currentToken.Type))
            {
                var op = _currentToken;
                Eat(op.Type);
                return new BinOp(left, op, Additive());
            }

            return left;
        }

        private AstNode Additive()
        {
            var left = Multiplicative();
            while (new[] {TokenType.Plus, TokenType.Minus}.Contains(_currentToken.Type))
            {
                var op = _currentToken;
                Eat(op.Type);
                left = new BinOp(left, op, Multiplicative());
            }

            return left;
        }

        private AstNode Multiplicative()
        {
            var left = Unary();
            while (new[] {TokenType.Mul, TokenType.Div, TokenType.Remainder}.Contains(_currentToken.Type))
            {
                var op = _currentToken;
                Eat(op.Type);
                left = new BinOp(left, op, Unary());
            }

            return left;
        }

        private AstNode Unary()
        {
            if (new[] {TokenType.Plus, TokenType.Minus, TokenType.BitwiseNot, TokenType.Negate}.Contains(_currentToken
                .Type))
            {
                var op = _currentToken;
                Eat(op.Type);
                return new UnaryOp(op, Factor());
            }

            return Factor();
        }

        private AstNode Factor()
        {
            switch (_currentToken.Type)
            {
                case TokenType.Integer:
                {
                    var token = _currentToken;
                    Eat(TokenType.Integer);
                    return new Num(token);
                }
                case TokenType.Double:
                {
                    var token = _currentToken;
                    Eat(TokenType.Double);
                    return new Num(token);
                }
                case TokenType.True:
                {
                    var token = _currentToken;
                    Eat(TokenType.True);
                    return new Bool(token);
                }
                case TokenType.False:
                {
                    var token = _currentToken;
                    Eat(TokenType.False);
                    return new Bool(token);
                }
                case TokenType.Continue:
                {
                    var token = _currentToken;
                    Eat(TokenType.Continue);
                    return new Continue(token);
                }
                case TokenType.Break:
                {
                    var token = _currentToken;
                    Eat(TokenType.Break);
                    return new Break(token);
                }
                case TokenType.Id:
                {
                    return Var();
                }
                case TokenType.Lparen:
                {
                    Eat(TokenType.Lparen);
                    var result = Expr();
                    Eat(TokenType.Rparen);
                    return result;
                }
                case TokenType.Plus:
                {
                    Eat(TokenType.Plus);
                    return Expr();
                }
                case TokenType.Minus:
                {
                    var op = _currentToken;
                    Eat(TokenType.Minus);
                    return new UnaryOp(op, Expr());
                }

                default:
                    throw new Exception($"Unknown token type found: {_currentToken.Type}");
            }

        }
        
        private AstNode Var()
        {
            var token = _currentToken;
            Eat(TokenType.Id);
            return new Variable(token);
        }
        
        private void Eat(TokenType tokenType, bool optional = false)
        {
            if (_currentToken.Type == tokenType)
            {
                _currentToken = _lexer.GetNextToken();
            }
            else if (!optional)
            {
                throw new Exception($"Expected token type {tokenType} but got {_currentToken.Type}");
            }
        }

    }
}