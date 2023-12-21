namespace PocketGoogle;

public class Indexer : IIndexer
{
    private Dictionary<string, Dictionary<int, List<int>>> wordsDict = new();
    private readonly char[] separators = { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };

    public void Add(int id, string documentText)
    {
        var wordIndexes = new Dictionary<string, List<int>>();
        var words = documentText.Split(separators);
        var position = 0;

        foreach (var word in words)
        {
            if (!wordIndexes.ContainsKey(word))
                wordIndexes.Add(word, new List<int>());
            wordIndexes[word].Add(position);
            position += word.Length + 1;
        }

        foreach (var word in wordIndexes)
        {
            if (!wordsDict.ContainsKey(word.Key))
                wordsDict[word.Key] = new Dictionary<int, List<int>>();
            wordsDict[word.Key][id] = word.Value;
        }
    }

    public List<int> GetIds(string word)
    {
        return wordsDict.ContainsKey(word) ? new List<int>(wordsDict[word].Keys) : new List<int>();
    }

    public List<int> GetPositions(int id, string word)
    {
        if (!wordsDict.ContainsKey(word))
            return new List<int>();
        return wordsDict[word].ContainsKey(id) ? wordsDict[word][id] : new List<int>();
    }

    public void Remove(int id)
    {
        foreach (var word in wordsDict.Keys.Where(word => wordsDict[word].ContainsKey(id)))
        {
            wordsDict[word].Remove(id);
        }
    }
}