using System;
using System.Collections.Generic;
using FourShardsScript.Symbols;

namespace FourShardsScript
{
    public class Context
    {
        private readonly SymbolTable _symbols;
        private readonly Context _parent;

        private readonly Dictionary<string, object> _memory = new Dictionary<string, object>();

        public Context(SymbolTable symbols, Context parent = null)
        {
            _symbols = symbols;
            _parent = parent;
        }

        public object this[string key]
        {
            get
            {
                var symbol = _symbols.Lookup(key);
                if (symbol != null)
                {
                    switch (symbol)
                    {
                        case FunctionSymbol fs:
                            return fs.FunctionDecl;
                        default:
                            return _memory.ContainsKey(key) ? _memory[key] : null;
                    }
                }

                if (_parent != null)
                {
                    return _parent[key];
                }

                throw new Exception($"Variable declaration not found: {key}");
            }
            set
            {
                var symbol = _symbols.Lookup(key);
                if (symbol != null)
                {
                    if (symbol is LetSymbol || symbol is FormalArgumentSymbol)
                    {
                        _memory[key] = value;
                    }
                    else
                    {
                        throw new Exception($"Cannot assign value to symbol of type {symbol.GetType()}");
                    }
                }
                else if (_parent != null)
                {
                    _parent[key] = value;
                }
                else
                {
                    throw new Exception($"Variable declaration not found: {key}");
                }
            }
        }

        public void SetConstantValue(string name, object value)
        {
            var symbol = _symbols.Lookup(name);
            if (symbol is ConstSymbol)
            {
                _memory[name] = value;
            }
            else
            {
                throw new Exception($"Setting non-const variable as constant {name}");
            }
            
        }
    }
}