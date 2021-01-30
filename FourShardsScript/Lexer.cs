using System;
using FourShardsScript.Extensions;
using System.Collections.Generic;

namespace FourShardsScript
{
    public class Lexer
    {

        private static readonly IDictionary<string, Token> ReservedKeywords = new Dictionary<string, Token>()
        {
            ["true"] = new Token(TokenType.True, true),
            ["false"] = new Token(TokenType.False, false),
            ["instanceof"] = new Token(TokenType.Instanceof, "instanceof"),
            ["const"] = new Token(TokenType.Const, "const"),
            ["let"] = new Token(TokenType.Let, "let"),
            ["if"] = new Token(TokenType.If, "if"),
            ["else"] = new Token(TokenType.Else, "else"),
            ["for"] = new Token(TokenType.For, "for"),
            ["while"] = new Token(TokenType.While, "while"),
            ["function"] = new Token(TokenType.FunctionDecl, "function"),
            ["return"] = new Token(TokenType.Return, "return"),
            ["break"] = new Token(TokenType.Break, "break"),
            ["continue"] = new Token(TokenType.Continue, "continue"),
        };

        private readonly string _text;
        private int _pos;
        private char? _currentChar;
        private readonly List<Token> _peekCache;

        public Lexer(string text)
        {
            _text = text;
            _pos = 0;
            _currentChar = _text[_pos];
            _peekCache = new List<Token>();
        }
        
        private void Advance()
        {
            _pos++;
            if (_pos > _text.Length - 1)
            {
                _currentChar = null;
            }
            else
            {
                _currentChar = _text[_pos];
            }
        }

        public Token Peek(int index = 0)
        {
            var i = _peekCache.Count;
            while (i <= index)
            {
                _peekCache.Add(GetNextToken(false));
                i++;
            }

            return _peekCache[index];
        }
        
        private void SkipWhitespace()
        {
            while (_currentChar != null && char.IsWhiteSpace(_currentChar.Value))
            {
                Advance();
            }
        }

        private Token Integer()
        {
            var number = "";
            while (_currentChar != null && char.IsDigit(_currentChar.Value))
            {
                number = $"{number}{_currentChar}";
                Advance();
            }

            if (_currentChar != null && '.' == _currentChar.Value)
            {
                number = $"{number}{_currentChar}";
                Advance();
                while (_currentChar != null && char.IsDigit(_currentChar.Value))
                {
                    number = $"{number}{_currentChar}";
                    Advance();
                }

                return new Token(TokenType.Double, double.Parse(number));
            }

            return new Token(TokenType.Integer, int.Parse(number));

        }

        private Token Id()
        {
            var result = "";
            while (_currentChar != null && _currentChar.IsIdentifierPart())
            {
                result = $"{result}{_currentChar.Value}";
                Advance();
            }

            return ReservedKeywords.ContainsKey(result) ? ReservedKeywords[result] : new Token(TokenType.Id, result);
        }
        
        public Token GetNextToken(bool useCache = true)
        {
            switch (_peekCache.Count > 0)
            {
                case true when useCache:
                {
                    var token = _peekCache[0];
                    _peekCache.RemoveAt(0);
                    return token;
                }
            }
            
            while (_currentChar != null)
            {
                if (char.IsWhiteSpace(_currentChar.Value))
                {
                    SkipWhitespace();
                    continue;
                }
                
                if (char.IsDigit(_currentChar.Value))
                {
                    return Integer();
                }

                if ((_currentChar.IsIdentifierStart()))
                {
                    return Id();
                }

                switch (_currentChar)
                {
                    case '|':
                        Advance();
                        switch (_currentChar)
                        {
                            case '|':
                            {
                                Advance();
                                if (_currentChar != '=') return new Token(TokenType.LogicalOr, "||");
                                Advance();
                                return new Token(TokenType.Assign, "||=");
                            }
                            case '=':
                                Advance();
                                return new Token(TokenType.Assign, "|=");
                            default:
                                return new Token(TokenType.BitwiseOr, '|');
                        }

                    case '&':
                        Advance();
                        switch (_currentChar)
                        {
                            case '&':
                            {
                                Advance();

                                if (_currentChar != '=') return new Token(TokenType.LogicalAnd, "&&");
                                Advance();
                                return new Token(TokenType.Assign, "&&=");
                            }
                            case '=':
                                Advance();
                                return new Token(TokenType.Assign, "&=");
                            default:
                                return new Token(TokenType.BitwiseAnd, '&');
                        }

                    case 'ˆ':
                        Advance();
                        if (_currentChar != '=') return new Token(TokenType.BitwiseXor, 'ˆ');
                        Advance();
                        return new Token(TokenType.Assign, "ˆ=");
                    case '=':
                        Advance();
                        if (_currentChar != '=') return new Token(TokenType.Assign, "=");
                        Advance();
                        return new Token(TokenType.Equals_, "==");
                    case '!':
                        Advance();
                        if (_currentChar != '=') return new Token(TokenType.Negate, '!');
                        Advance();
                        return new Token(TokenType.NotEquals, "!=");
                    case '<':
                        Advance();
                        switch (_currentChar)
                        {
                            case '<':
                            {
                                Advance();
                                if (_currentChar != '=') return new Token(TokenType.ShiftLeft, "<<");
                                Advance();
                                return new Token(TokenType.Assign, "<<=");
                            }
                            case '=':
                                Advance();
                                return new Token(TokenType.LessOrEquals, "<=");
                            default:
                                return new Token(TokenType.Less, '<');
                        }

                    case '>':
                        Advance();
                        switch (_currentChar)
                        {
                            case '>':
                            {
                                Advance();
                                switch (_currentChar)
                                {
                                    case '>':
                                    {
                                        Advance();
                                        if (_currentChar != '=') return new Token(TokenType.UnsignedShiftRight, ">>>");
                                        Advance();
                                        return new Token(TokenType.Assign, ">>>=");
                                    }
                                    case '=':
                                        Advance();
                                        return new Token(TokenType.Assign, ">>=");
                                    default:
                                        return new Token(TokenType.ShiftRight, ">>");
                                }
                            }
                            case '=':
                                Advance();
                                return new Token(TokenType.GreaterOrEqual, ">=");
                            default:
                                return new Token(TokenType.Greater, '>');
                        }

                    case '+': 
                        Advance();
                        switch (_currentChar)
                        {
                            case '=':
                                Advance();
                                return new Token(TokenType.Assign, "+=");
                            default:
                                return new Token(TokenType.Plus, "+");
                        }
                    case '-':
                        Advance();
                        switch (_currentChar)
                        {
                            case '=':
                                Advance();
                                return new Token(TokenType.Assign, "-=");
                            default:
                                return new Token(TokenType.Minus, '-');
                        }
                    case '*':
                        Advance();
                        switch (_currentChar)
                        {
                            case '*':
                            {
                                Advance();

                                if (_currentChar != '=') return new Token(TokenType.Exponential, "**");
                                Advance();
                                return new Token(TokenType.Assign, "**=");
                            }
                            case '=':
                                Advance();
                                return new Token(TokenType.Assign, "*=");
                            default:
                                return new Token(TokenType.Mul, '*');
                        }

                    case '/':
                        Advance();
                        switch (_currentChar)
                        {
                            case '=':
                                Advance();
                                return new Token(TokenType.Assign, "/=");
                            default:
                                return new Token(TokenType.Div, '/');
                        }
                    case '%':
                        Advance();
                        switch (_currentChar)
                        {
                            case '=':
                                Advance();
                                return new Token(TokenType.Assign, "%=");
                            default:
                                return new Token(TokenType.Remainder, '%');
                        }
                    case '˜':
                        Advance();
                        switch (_currentChar)
                        {
                            case '=':
                                Advance();
                                return new Token(TokenType.Assign, "˜=");
                            default:
                                return new Token(TokenType.BitwiseNot, '˜');
                        }
                    case '?':
                        Advance();
                        switch (_currentChar)
                        {
                            case '.':
                                Advance();
                                return new Token(TokenType.NullSafeMemberAccess, "?.");
                            case '?':
                                Advance();
                                return new Token(TokenType.NullishCoalescing, "??");
                            default:
                                return new Token(TokenType.QuestionMark, '?');
                        }

                    case ':':
                        Advance();
                        return new Token(TokenType.Colon, ':');
                    case ';':
                        Advance();
                        return new Token(TokenType.Semicolon, ';');
                    case ',':
                        Advance();
                        return new Token(TokenType.Comma, ',');
                    case '(':
                        Advance();
                        return new Token(TokenType.Lparen, '(');
                    case ')':
                        Advance();
                        return new Token(TokenType.Rparen, ')');
                    case '{':
                        Advance();
                        return new Token(TokenType.Lcparen, '{');
                    case '}':
                        Advance();
                        return new Token(TokenType.Rcparen, '}');
                }

                throw new Exception($"Unknown character found in input: {_currentChar}");
            }

            return new Token(TokenType.Eof, null);
        }
        
    }
}