using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public class TextProcessor
    {
        private static Dictionary<string, string> shiftedCharacterMap = new Dictionary<string, string>()
        {
            { "a", "A" },
            { "b", "B" },
            { "c", "C" },
            { "d", "D" },
            { "e", "E" },
            { "f", "F" },
            { "g", "G" },
            { "h", "H" },
            { "i", "I" },
            { "j", "J" },
            { "k", "K" },
            { "l", "L" },
            { "m", "M" },
            { "n", "N" },
            { "o", "O" },
            { "p", "P" },
            { "q", "Q" },
            { "r", "R" },
            { "s", "S" },
            { "t", "T" },
            { "u", "U" },
            { "v", "V" },
            { "w", "W" },
            { "x", "X" },
            { "y", "Y" },
            { "z", "Z" },
            { "`", "~" },
            { "1", "!" },
            { "2", "@" },
            { "3", "#" },
            { "4", "$" },
            { "5", "%" },
            { "6", "^" },
            { "7", "&" },
            { "8", "*" },
            { "9", "(" },
            { "0", ")" },
            { "-", "_" },
            { "=", "+" },
            { ",", "<" },
            { ".", ">" },
            { "/", "?" },
            { ";", ":" },
            { "'", "\"" },
            { "\\", "|" },
            { "[", "{" },
            { "]", "}" },
        };

        public static string GetShiftedCharacter(string character)
        {
            if (shiftedCharacterMap.ContainsKey(character))
                return shiftedCharacterMap[character];
            return character;
        }

        public static string GetShiftedCharacter(char character)
        {
            return GetShiftedCharacter(character.ToString());
        }
    }
}
