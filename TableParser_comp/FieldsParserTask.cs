using System.Collections.Generic;
using NUnit.Framework;

namespace TableParser;

[TestFixture]
public class FieldParserTaskTests
{
    [TestCase("text", new[] { "text" })]
    [TestCase("goodbye world", new[] { "goodbye", "world" })]
    [TestCase("'x y'", new[] { "x y" })]
    [TestCase("'x ", new[] { "x " })]
    [TestCase(@"""a 'b' 'c' d""", new[] { "a 'b' 'c' d" })]
    [TestCase(@"a""b c d e""", new[] { "a", "b c d e" })]
    [TestCase(@"""b c d e""f", new[] { "b c d e", "f" })]
    [TestCase(@"""a \""c\""""", new[] { @"a ""c""" })]
    [TestCase(@"""\\""", new[] { "\\" })]
    [TestCase(@" '' ", new[] { "" })]
    [TestCase(@"'a\'a\'a'", new[] { "a'a'a" })]
    [TestCase(@"'""1""", new[] { @"""1""" })]
    [TestCase(" 1 ", new[] { "1" })]
    [TestCase("", new string[] { })]
    [TestCase("    ", new string[] { })]
    [TestCase("   multiple     spaces   ", new string[] { "multiple", "spaces" })]
    [TestCase("   hi_bro   ", new string[] { "hi_bro" })]
    [TestCase("   a     b   c   ", new string[] { "a", "b", "c" })]
    public static void Test(string input, string[] expectedResult)
	{
		var actualResult = FieldsParserTask.ParseLine(input);
		Assert.AreEqual(expectedResult.Length, actualResult.Count);
		for (int i = 0; i < expectedResult.Length; ++i)
		{
			Assert.AreEqual(expectedResult[i], actualResult[i].Value);
		}
	}

	// Скопируйте сюда метод с тестами из предыдущей задачи.
}

public class FieldsParserTask
{
	// При решении этой задаче постарайтесь избежать создания методов, длиннее 10 строк.
	// Подумайте как можно использовать ReadQuotedField и Token в этой задаче.
	public static List<Token> ParseLine(string line)
	{
		var result = new List<Token>();
		var index = 0;
		while (index < line.Length)
		{
			while (line[index] == ' ')
			{
				index++;
			}
			var field = ReadField(line, index);
			result.Add(field);
			index = field.GetIndexNextToToken();
		}
		return result;
		//return new List<Token> { ReadQuotedField(line, 0) }; // сокращенный синтаксис для инициализации коллекции.
	}
        
	private static Token ReadField(string line, int startIndex)
	{
		if (line[startIndex] == '"' || line[startIndex] == '\'') 
		{ 
			return ReadQuotedField(line, startIndex);
		}
		var index = startIndex;
        while (index < line.Length && line[index] != ' ' && line[index] != '"' && line[index] != '\'')
        {
			index++;
        }
		var length = index - startIndex;
		return new Token(line.Substring(startIndex, length), startIndex, length);
        //return new Token(line, 0, line.Length);
    }

	public static Token ReadQuotedField(string line, int startIndex)
	{
		return QuotedFieldTask.ReadQuotedField(line, startIndex);
	}
}