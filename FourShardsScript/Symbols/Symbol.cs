namespace FourShardsScript.Symbols
{
    public class Symbol
    {
        public string Name { get; }
        public TypeSymbol Type { get; }

        public Symbol(string name, TypeSymbol type = null)
        {
            Name = name;
            Type = type;
        }
    }
}