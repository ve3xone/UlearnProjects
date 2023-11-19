namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(
            Dictionary<string, string> nextWords,
            string phraseBeginning,
            int wordsCount)
        {
            var words = phraseBeginning.Split().ToList();
            var preWord = words.Count > 0 ? words[^1] : "";
            var prePreWord = words.Count > 1 ? words[^2] : "";

            for (var i = 0; i < wordsCount; i++)
            {
                if (TryGetNextWord(nextWords, prePreWord, preWord, out var nextWord))
                {
                    break;
                }
                else
                {
                    words.Add(nextWord);
                    prePreWord = preWord;
                    preWord = nextWord;
                }
            }

            return string.Join(" ", words);
        }

        private static bool TryGetNextWord(Dictionary<string, string> nextWords, string prePreWord, string preWord, out string nextWord)
        {
            nextWord = null;
            var begin = prePreWord + " " + preWord;

            if (nextWords.ContainsKey(begin))
            {
                nextWord = nextWords[begin];
                return false;
            }
            else if (nextWords.ContainsKey(preWord))
            {
                nextWord = nextWords[preWord];
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
