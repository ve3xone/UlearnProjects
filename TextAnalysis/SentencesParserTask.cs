using System.Text;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>();
            var delimiters = ".!?;:()".ToCharArray();
            var sentences = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            foreach (var sentence in sentences)
            {
                var words = ParseSentence(sentence);
                if (words.Count > 0)
                    sentencesList.Add(words);
            }

            return sentencesList;
        }

        public static List<string> ParseSentence(string sentence)
        {
            var words = new List<string>();
            var wordBuilder = new StringBuilder();

            foreach (var symbol in sentence)
            {
                if (char.IsLetter(symbol) || symbol == '\'')
                    wordBuilder.Append(char.ToLower(symbol));
                else
                {
                    if (wordBuilder.Length > 0)
                        words.Add(wordBuilder.ToString());
                    wordBuilder.Clear();
                }
            }

            if (wordBuilder.Length > 0)
                words.Add(wordBuilder.ToString());
            wordBuilder.Clear();

            return words;
        }
    }
}