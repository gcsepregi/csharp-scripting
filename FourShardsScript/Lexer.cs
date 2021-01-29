using System;

namespace FourShardsScript
{
    public class Lexer
    {
        
        private readonly string _text;
        private int _pos;
        private char? _currentChar;

        public Lexer(string text)
        {
            _text = text;
            _pos = 0;
            _currentChar = _text[_pos];
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

        private void SkipWhitespace()
        {
            while (_currentChar != null && char.IsWhiteSpace(_currentChar.Value))
            {
                Advance();
            }
        }

        private int Integer()
        {
            var result = "";
            while (_currentChar != null && char.IsDigit(_currentChar.Value))
            {
                result = $"{result}{_currentChar}";
                Advance();
            }

            return int.Parse(result);
        }

        public Token GetNextToken()
        {

            while (_currentChar != null)
            {
                if (char.IsWhiteSpace(_currentChar.Value))
                {
                    SkipWhitespace();
                    continue;
                }
                
                if (char.IsDigit(_currentChar.Value))
                {
                    return new Token(TokenType.Integer,  Integer());
                }

                switch (_currentChar)
                {
                    case '+': 
                        Advance();
                        return new Token(TokenType.Plus, '+');
                    case '-':
                        Advance();
                        return new Token(TokenType.Minus, '-');
                    case '*':
                        Advance();
                        return new Token(TokenType.Mul, '*');
                    case '/':
                        Advance();
                        return new Token(TokenType.Div, '/');
                    case '(':
                        Advance();
                        return new Token(TokenType.Lparen, '(');
                    case ')':
                        Advance();
                        return new Token(TokenType.Rparen, ')');
                }

                throw new Exception($"Unknown character found in input: {_currentChar}");
            }

            return new Token(TokenType.Eof, null);
        }
        
    }
}