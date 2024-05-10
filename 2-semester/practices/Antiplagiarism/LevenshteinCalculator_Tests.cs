using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Antiplagiarism;

[TestFixture]
public class LevenshteinCalculator_Tests
{
	private LevenshteinCalculator levenshteinCalculator;

	[SetUp]
	public void SetUp()
	{
		levenshteinCalculator = new LevenshteinCalculator();
	}

	public const string Ax10 = "a a a a a a a a a a";
	public const string Bx10 = "b b b b b b b b b b";

	[Order(0)]
	[TestCase("aa", "bb", 1)]
	[TestCase("aa", "ab", 0.5)] 
	[TestCase("aaaa", "abbb", 0.5)]
	[TestCase("abcd", "1234", 1)]
	[TestCase("abcd", "a1234", 7.0 / 8)]
	[TestCase("abcd", "aaab234", 5.0 / 7)]
	public void Use_GetTokenDistance_ToCompareSingleTokens(string document1Token, string document2Token, double expectedDistance)
	{
		var first = new List<string> { document1Token };
		var second = new List<string> { document2Token };
		var actual = LevenshteinCalculator.CompareDocumentsPairwise(
			new List<List<string>> { first, second }).Single();
		Assert.AreEqual(expectedDistance, actual.Distance, 1e-6);
	}

	[Order(1)]
	[TestCase("a", "a", 0)]
	[TestCase("a", "a a", 1)]
	[TestCase("a a", "a", 1)]
	[TestCase("к о т", "к л о н", 2)]
	[TestCase("y a b x", "a b c d", 3)]
	[TestCase("с т у д е н т", "с о л д а т", 4)]
	[TestCase("д и н п р о г", "с л о ж н о", 6)]
	[TestCase(Ax10, Bx10, 10)]
	[TestCase(Ax10, "a", 9)]
	[TestCase("a", Ax10, 9)]
	[TestCase(Ax10, "a a", 8)]
	[TestCase("a a", Ax10, 8)]
	[TestCase(Ax10 + " " + Bx10, "a a a a b b b", 13)]
	[TestCase("aa bb", "aa ab", 0.5)]
	[TestCase("abb cbaa bca", "ccb ca", 5.0 / 3)]
	[TestCase("aa bbcc aa bba", "aa cc aa cc bba", 1.5)]
	[TestCase("ab cr e f", "abf jjc a re", 3)]
	[TestCase("abcde crzsd efdre fghij", "abcdef ghijc auoe reij", 2.626984126984127)]
	[TestCase("hrc**&) + 129x", "rc& 123 987 *+ - ** / << 213", 7.6)]
	public void CompareTwoDocuments(string document1Token, string document2Token, double expectedDistance)
	{
		var first = document1Token.Split(' ')
			.ToList();
		var second = document2Token.Split(' ')
			.ToList();
		var actual = LevenshteinCalculator.CompareDocumentsPairwise(
			new List<List<string>> { first, second }).Single();
		Assert.AreEqual(expectedDistance, actual.Distance, 1e-6);
	}

	[TestCase(new string[0], new double[0], TestName = "no documents")]
	[TestCase(new[] { "singleDocument" }, new double[] { }, TestName = "single document")]
	[TestCase(new[] { "a", "b" }, new double[] { 1 }, TestName = "2 documents")]
	[TestCase(new[] { "a a", "a b", "b b" }, new double[] { 1, 1, 2 }, TestName = "3 documents")]
	[TestCase(new[] { "a b b b a b a", "a a b b b a b b", "a a a a a a a", "b b b b b b", "a b b a b b a" },
		new double[] { 2, 2, 3, 3, 3, 3, 4, 4, 5, 7 }, TestName = "5 documents")]
	[Order(2)]
	public void ReturnAllPairwiseDistances(IList<string> documents, params double[] expectedPairwiseDistances)
	{
		var tokenizedDocuments = documents.Select(document => document.Split(' ')
			.ToList()).ToList();
		var actual = LevenshteinCalculator.CompareDocumentsPairwise(tokenizedDocuments)
			.Select(r => r.Distance);
		// Сравнение коллекций без учета порядка:
		CollectionAssert.AreEquivalent(expectedPairwiseDistances, actual);
	}

	[Test]
	[Order(3)]
	public void ReturnAllPairwiseComparisons()
	{
		var documents = new List<string> { "a a", "a b", "b b" };
		var tokenizedDocuments = documents.Select(document => document.Split(' ')
			.ToList()).ToList();
		var expected =
			new List<ComparisonResult>
			{
				new(tokenizedDocuments[0], tokenizedDocuments[1], 1),
				new(tokenizedDocuments[0], tokenizedDocuments[2], 2),
				new(tokenizedDocuments[1], tokenizedDocuments[2], 1),
			};
		var actual = LevenshteinCalculator.CompareDocumentsPairwise(tokenizedDocuments);
		// Сравнение коллекций без учета порядка:
		CollectionAssert.AreEquivalent(expected, actual);
	}
}