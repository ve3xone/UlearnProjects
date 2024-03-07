using NUnit.Framework;
using System.Text;

namespace TableParser;

[TestFixture]
public class QuotedFieldTaskTests
{
    [TestCase("''", 0, "", 2)]
    [TestCase("'a'", 0, "a", 3)]
    [TestCase("hi\"bro\"", 0, "bro", 3)]
    [TestCase("hi\"bro", 0, "bro", 3)]
    [TestCase(@"some_text ""QF \"""" other_text", 10, "QF \"", 7)]
    public void Test(string line, int startIndex, string expectedValue, int expectedLength)
    {
        var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
        Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
    }
}

class QuotedFieldTask
{
    public static Token ReadQuotedField(string line, int startIndex)
    {
        var openQuote = line[startIndex];
        var index = startIndex + 1;
        var builder = new StringBuilder();

        while (index < line.Length && line[index] != openQuote)
        {
            if (line[index] == '\\' && ++index < line.Length)
            {
                builder.Append(line[index]);
            }
            else
            {
                builder.Append(line[index]);
            }
            index++;
        }

        if (index < line.Length)
        {
            index++;
        }

        return new Token(builder.ToString(), startIndex, index - startIndex);
    }
}
