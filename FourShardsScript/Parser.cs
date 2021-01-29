using System;
using System.Linq;
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

        public IAstNode Parse()
        {
            return Expr();
        }
        
        private void Eat(TokenType tokenType)
        {
            if (_currentToken.Type == tokenType)
            {
                _currentToken = _lexer.GetNextToken();
            }
            else
            {
                throw new Exception($"Expected token type {tokenType} but got {_currentToken.Type}");
            }
        }

        private IAstNode Factor()
        {
            var token = _currentToken;
            switch (token.Type)
            {
                case TokenType.Plus:
                    Eat(TokenType.Plus);
                    return new UnaryOp(token, Factor());
                case TokenType.Minus:
                    Eat(TokenType.Minus);
                    return new UnaryOp(token, Factor());
                case TokenType.Integer:
                    Eat(TokenType.Integer);
                    return new Num(token);
                case TokenType.Lparen:
                    Eat(TokenType.Lparen);
                    var node = Expr();
                    Eat(TokenType.Rparen);
                    return node;
                default:
                    throw new Exception($"Expected token types: Integer, Lparen. Got: {token.Type}");
            }
        }

        private IAstNode Term()
        {
            var node = Factor();
            while (new[] {TokenType.Mul, TokenType.Div}.Contains(_currentToken.Type))
            {
                var token = _currentToken;
                if (token.Type == TokenType.Mul)
                {
                    Eat(TokenType.Mul);
                }
                else if (token.Type == TokenType.Div)
                {
                    Eat(TokenType.Div);
                }

                node = new BinOp(node, token, Factor());
            }

            return node;
        }

        private IAstNode Expr()
        {
            var node = Term();

            while (new [] {TokenType.Plus, TokenType.Minus}.Contains(_currentToken.Type))
            {
                var token = _currentToken;
                if (token.Type == TokenType.Plus)
                {
                    Eat(TokenType.Plus);
                }
                else if (token.Type == TokenType.Minus)
                {
                    Eat(TokenType.Minus);
                }

                node = new BinOp(node, token, Term());
            }
            return node;
        }

    }
}