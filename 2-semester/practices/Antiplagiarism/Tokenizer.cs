using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Antiplagiarism;

public static class Tokenizer
{
	private static readonly Regex regex = new(@"(\w+)|(\r?\n)|(.)", RegexOptions.IgnorePatternWhitespace);

	public static IEnumerable<string> Tokenize(string text)
	{
		var matches = regex.Matches(text);
		foreach (Match match in matches)
		{
			if (match.Success)
			{
				yield return match.Value;
			}
		}
	}
}