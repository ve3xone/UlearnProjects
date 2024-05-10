using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Antiplagiarism;

[TestFixture]
public class LongestCommonSubsequenceCalculator_Tests
{
	public const string Ax10 = "a a a a a a a a a a";
	public const string Bx10 = "b b b b b b b b b b";

	[TestCase("a", "a", 1)]
	[TestCase("a", "b", 0)]
	[TestCase("a", "", 0)]
	[TestCase("", "b", 0)]
	[TestCase("a", "a a", 1)]
	[TestCase("a a", "a b", 1)]
	[TestCase("a b", "a a b b", 2)]
	[TestCase("a a b b", "a b", 2)]
	public void TestSimple(string first, string second, int expectedSubsequenceLength)
		=> DoTest(first, second, expectedSubsequenceLength);

	[TestCase("к о т", "к л о н", 2)]
	[TestCase("y a b x", "a b c d", 2)]
	[TestCase("с т у д е н т", "с о л д а т", 3)]
	public void TestWords(string first, string second, int expectedSubsequenceLength)
		=> DoTest(first, second, expectedSubsequenceLength);

	[TestCase(Ax10, Bx10, 0)]
	[TestCase(Ax10, "a", 1)]
	[TestCase(Ax10, "a a", 2)]
	[TestCase(Ax10 + " " + Bx10, "a a a a a b b b b b", 10)]
	[TestCase("a b b b a b a", "a a b b b a b b", 6)]
	[TestCase("a b b b a b a", "a a a a a a a", 3)]
	[TestCase("a b b b a b a", "b b b b b b", 4)]
	[TestCase("a b b b a b a", "a b b a b b a", 6)]
	[TestCase("b b b b a a b b b a", "a a a b b b a a a b", 6)]
	[TestCase("b a a b a b b a b b a b b a b", "b b b a b a b b b b a a a b b", 11)]
	[TestCase(
		"b b a b b a a b b b b a a a a a a b a b",
		"a a b a b b b b a b a a b a a b b a a a", 14)]
	[TestCase(
		"a b a a b b b b b b b b b b a a a a b b b a b b a",
		"a b b b b a b a a b b a a a b a a b a a b b a a a", 17)]
	[TestCase(
		"b a a b b a a b a b b b a a b a a a a a b a b b b a b b a a",
		"a a b b a a b a a a b b b a a a b a a b a b a b b b a b a a", 26)]
	[TestCase(
		"a b b b a b a a b a b b a b b a a b a b b b a b a b a b b a b a b b a b a",
		"a a a b b b b b b b a a a b a b a a b a a b b a b b a", 24)]
	[TestCase(
		"b a b a a b b b a b b a b b b b a a a b a b b b b a b b b a a a b a b b b b a a b a b a b b a b b b",
		"b a b a b b b b b b b b b a b a a a b b a b b b b a b b b a a a b b a a b a b b b a a b b b a a a a b b",
		43)]
	public void TestBig(string first, string second, int expectedSubsequenceLength)
		=> DoTest(first, second, expectedSubsequenceLength);

	[TestCase("aa bb", "aa ab", 1)]
	[TestCase("abb cbaa ca", "ccb ca", 1)]
	[TestCase("aa bbcc aa bba", "aa cc aa cc bba", 3)]
	[TestCase("ab cr e f", "abf jjc a re", 0)]
	[TestCase("abc bca cab bba", "abc xx bca cab ba ca cab bba", 4)]
	[TestCase("hrc ** &) + 129x", "rc& 123 987 + - ** + << 129x", 3)]
	public void TestMultiLetter(string first, string second, int expectedSubsequenceLength)
		=> DoTest(first, second, expectedSubsequenceLength);

	private void DoTest(
		string first, string second,
		int expectedSubsequenceLength)
	{
		var firstSequence = first.Split(' ').ToList();
		var secondSequence = second.Split(' ').ToList();
		var actual = LongestCommonSubsequenceCalculator.Calculate(firstSequence, secondSequence);

		Assert.AreEqual(expectedSubsequenceLength, actual.Count);
		AssertSubsequence(actual, firstSequence);
		AssertSubsequence(actual, secondSequence);
		Assert.IsTrue(actual.All(firstSequence.Contains), // Contains будет сравнивать токены по ссылкам
			nameof(LongestCommonSubsequenceCalculator) + " должен возвращать токены первого документа");
	}

	private void AssertSubsequence(IList<string> subsequence, IList<string> sequence)
	{
		var i = 0;
		foreach (var token in sequence)
		{
			if (i == subsequence.Count) break;
			if (token == subsequence[i]) ++i;
		}

		if (i != subsequence.Count)
		{
			Assert.Fail($"Expected to be subsequence.\nSequence: <{string.Join(" ", sequence)}>, expected subsequence <{string.Join(" ", subsequence)}>");
		}
	}
}