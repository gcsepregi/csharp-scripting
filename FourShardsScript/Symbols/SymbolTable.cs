using System;
using System.Collections.Generic;

namespace FourShardsScript.Symbols
{
    public class SymbolTable
    {
        private readonly SymbolTable _parent;
        private readonly Dictionary<string, Symbol> _symbols = new Dictionary<string, Symbol>();

        public SymbolTable(SymbolTable parent = null)
        {
            _parent = parent;
        }
        
        public void Define(Symbol symbol)
        {
            if (_symbols.ContainsKey(symbol.Name))
            {
                throw new InvalidOperationException("Variable already declared");
            }
            _symbols[symbol.Name] = symbol;
        }

        public Symbol Lookup(string name, bool followParent = false)
        {
            return !_symbols.ContainsKey(name) ? followParent ? _parent?.Lookup(name, true) : null : _symbols[name];
        }
    }
}