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
		[Test]

        [TestCase(new string[] { "a", "b", "c" }, "ab", 1)]
        [TestCase(new string[] { "a", "b", "c" }, "dc", 3)]
        [TestCase(new string[] { "b", "ba", "c" }, "ab", 0)]
        [TestCase(new string[] { "a", "ab", "abc" }, "abc", 3)]
        [TestCase(new string[] { "a", "ab" }, "", 2)]
        [TestCase(new string[] { }, "abc", 0)]
        [TestCase(new string[] { "ab", "ab", "ab", "ab" }, "a", 4)]
		[TestCase(new string[] { "a", "ab", "abc" }, "abb", 2)]

        public void CheckRightBorder(string[] phrases, string prefix, int expectedIndex)
        {
            Assert.AreEqual(expectedIndex, RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Length));
        }

		[Test]

        [TestCase(new string[] { "a", "ab", "ac" }, "ba", 1)]
        [TestCase(new string[] { "aa", "ab", "ac" }, " ", 1)]
		[TestCase(new string[] { }, "ab", 1)]

        public void TopByPrefix_IsEmpty_WhenNoPhrases(string[] phrases, string prefix, int count)
        {
            CollectionAssert.IsEmpty(AutocompleteTask.GetTopByPrefix(phrases, prefix, count));
        }

		[Test]

        [TestCase(new string[] { "aa", "ab", "ac" }, " ", 0)]
        [TestCase(new string[] { "aa", "ab", "ac" }, "ab", 1)]
        [TestCase(new string[] { "aa", "ab", "ac" }, "b", 0)]
        [TestCase(new string[] { }, "a", 0)]
		[TestCase(new string[] { "aa", "ab", "ac" }, "", 3)]

        public void CountByPrefix_IsTotalCount_WhenEmptyPrefix(string[] phrases, string prefix, int expectedCount)
        {
            Assert.AreEqual(expectedCount, AutocompleteTask.GetCountByPrefix(phrases, prefix));
        }
    }
}