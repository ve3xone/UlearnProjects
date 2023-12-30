using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
			var index = GetStartIndex(phrases, prefix);

			if (index < phrases.Count && 
				phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];

            return null;
        }

        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            var startIndex = GetStartIndex(phrases, prefix);
            count = Math.Min(count, GetCountByPrefix(phrases, prefix));
            var result = new string[count];

            for (int i = 0; i < count; i++)
            {
                if (phrases[startIndex].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    result[i] = phrases[startIndex];
                startIndex++;
            }

            return result;
        }

        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            if (string.IsNullOrEmpty(prefix)) return phrases.Count;
            return RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count)
                   - LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) - 1;
        }

        private static int GetStartIndex(IReadOnlyList<string> phrases, string prefix)
        {
            return LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
        }
    }

    [TestFixture]
    public class AutocompleteTests
    {
        [TestCase(new string[] { }, "frieren")]
        [TestCase(new string[] { }, "lain")]
		[TestCase(new string[] { }, "misato")]

        public void TopByPrefix_IsEmpty_WhenNoPhrases(string[] phrases, string prefix, int count)
        {
            CollectionAssert.IsEmpty(AutocompleteTask.GetTopByPrefix(phrases, prefix, count));
        }

        [TestCase(new[] { "solo leveling", "one piece" }, "", 2)]
        [TestCase(new[] { "burn the witch", "vinland saga", "serial experiments lain" }, "", 3)]
		[TestCase(new string[] {  }, "", 0)]

        public void CountByPrefix_IsTotalCount_WhenEmptyPrefix(string[] phrases, string prefix, int expectedCount)
        {
            Assert.AreEqual(expectedCount, AutocompleteTask.GetCountByPrefix(phrases, prefix));
        }
    }
}