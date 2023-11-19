namespace TextAnalysis;

static class FrequencyAnalysisTask
{
    public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
    {
        var result = new Dictionary<string, string>();
        var freqDictionary = GetFreqDictionary(text); 
        foreach(var freq in freqDictionary) 
            result.Add(freq.Key, GetNextWord(freq.Value));
        return result;
    }

    private static Dictionary<string, Dictionary<string,int>> GetFreqDictionary(List<List<string>> text)
    {
        var result = new Dictionary<string, Dictionary<string,int>>();

        foreach(var sentence in text)
            for (var i = 0; i < sentence.Count - 1; i++)
            {
                var begin = sentence[i];
                var end = sentence[i + 1];
                UpdateFreq(result, begin, end);

                if(i < sentence.Count - 2)
                {
                    begin = sentence[i] + " " + sentence[i + 1];
                    end = sentence[i + 2];
                    UpdateFreq(result, begin, end);
                }
            }

        return result;
    }

    private static string GetNextWord(Dictionary<string,int> words)
    {
        var result = "";
        var maxFreq = 0;

        foreach(var pair in words)
            if (pair.Value > maxFreq || pair.Value == maxFreq && string.CompareOrdinal(pair.Key, result) < 0)
            {
                result = pair.Key;
                maxFreq = pair.Value;
            }

        return result;
    }

    private static void UpdateFreq(Dictionary <string, Dictionary<string,int>> result, string begin, string end)
    {
        if (!result.ContainsKey(begin))
            result.Add(begin, new Dictionary<string,int>());
        if (!result[begin].ContainsKey(end))
            result[begin].Add(end, 0);
        result[begin][end]++;
    }
}