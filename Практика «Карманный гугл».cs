using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        private readonly Dictionary<string, Dictionary<int, List<int>>> indexes = new();
		private readonly HashSet<char> separators = 
					 new HashSet<char> { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };

        public void Add(int id, string documentText)
        {
            var wordBuilder = new StringBuilder();
            var position = 0;

            for (int i = 0; i < documentText.Length; i++)
            {
                var symbol = documentText[i];

                if (separators.Contains(symbol))
                {
                    UpdateIndexes(id, wordBuilder, position);
                    position = i + 1;
                }
                else
                {
                    wordBuilder.Append(symbol);
                }
            }

            UpdateIndexes(id, wordBuilder, position);
        }

        private void UpdateIndexes(int id, StringBuilder wordBuilder, int wordStartIndex)
        {
            var word = wordBuilder.ToString();

            if (!indexes.TryGetValue(word, out var idDictionary))
            {
                idDictionary = new Dictionary<int, List<int>>();
                indexes[word] = idDictionary;
            }

            if (!idDictionary.ContainsKey(id))
            {
                idDictionary[id] = new List<int>();
            }

            idDictionary[id].Add(wordStartIndex);
            wordBuilder.Clear();
        }

        public List<int> GetIds(string word)
        {
            return indexes.TryGetValue(word, out var idDictionary) ? idDictionary.Keys.ToList() : new List<int>();
        }

        public List<int> GetPositions(int id, string word)
        {
			return indexes.TryGetValue(word, out var idDictionary) && 
			   	   idDictionary.TryGetValue(id, out var positions) ? positions : new List<int>();
		}

        public void Remove(int id)
        {
            var keysToRemove = indexes.Keys
                .Where(word => indexes[word].ContainsKey(id))
                .ToList();

            foreach (var key in keysToRemove)
            {
                indexes[key][id].Clear();
                indexes[key].Remove(id);
            }

			indexes.Keys.Where(word => indexes[word].Count == 0)
						.ToList().ForEach(emptyKey => indexes.Remove(emptyKey));
        }
    }
}