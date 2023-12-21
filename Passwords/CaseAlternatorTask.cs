namespace Passwords
{
    public class CaseAlternatorTask
    {
        public static List<string> AlternateCharCases(string lowercaseWord)
        {
            var result = new List<string>();
            AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
            return result;
        }

        static void AlternateCharCases(char[] word, int startIndex, List<string> result)
        {
            if (startIndex == word.Length)
            {
                result.Add(new string(word));
                return;
            }

            var original = word[startIndex];
            var upper = char.ToUpper(original);

            // Add both upper and lower case variations
            word[startIndex] = upper;
            AlternateCharCases(word, startIndex + 1, result);

            word[startIndex] = char.ToLower(original);
            AlternateCharCases(word, startIndex + 1, result);

            // Restore the original character
            word[startIndex] = original;
        }
    }
}