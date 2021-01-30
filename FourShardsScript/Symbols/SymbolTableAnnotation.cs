namespace FourShardsScript.Symbols
{
    public class SymbolTableAnnotation : IAnnotation
    {
        public SymbolTable Symbols { get; }

        public SymbolTableAnnotation(SymbolTable symbols)
        {
            Symbols = symbols;
        }
    }
}