namespace FourShardsScript.Extensions
{
    public static class CharExtensions
    {

        public static bool IsIdentifierStart(this char? character)
        {
            return character != null && (char.IsLetter(character.Value) || "$_".Contains(character.Value));
        }

        public static bool IsIdentifierPart(this char? character)
        {
            return character != null && (character.IsIdentifierStart() || char.IsDigit(character.Value));
        }

    }
    
}