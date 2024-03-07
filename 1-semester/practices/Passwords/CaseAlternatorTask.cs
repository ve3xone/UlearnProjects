using System.Collections.Generic;

namespace Passwords
{
    public class CaseAlternatorTask
    {
        public static List<string> AlternateCharCases(string lowercaseWord)
        {
            var result = new HashSet<string>();
            AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
            return new List<string>(result);
        }

        static void AlternateCharCases(char[] word, int startIndex, HashSet<string> result)
        {
            if (startIndex == word.Length)
            {
                result.Add(new string(word));
                return;
            }

            AlternateCharCases(word, startIndex + 1, result);
            var original = word[startIndex];
            var upper = char.ToUpper(original);
            if (char.IsLetter(original) && original != upper)
            {
                word[startIndex] = upper;
                AlternateCharCases(word, startIndex + 1, result);
                word[startIndex] = original;
            }
        }
    }
}