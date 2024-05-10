using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Antiplagiarism;

public class DocumentContent
{
	public string DocumentName;
	public List<string> Tokens;
	private readonly List<string> textWithWhiteSpaces;

	public DocumentContent(string documentName)
	{
		var text = File.ReadAllText(documentName);
		DocumentName = Path.GetFileNameWithoutExtension(documentName);
		textWithWhiteSpaces = Tokenizer.Tokenize(text).ToList();
		Tokens = textWithWhiteSpaces
			.Where(token => token.All(c => !char.IsWhiteSpace(c)))
			.ToList();
	}

	public IEnumerable<Tuple<string, TokenType>> DevideToCommonAndSpecificTokens(List<string> commonTokens)
	{
		var i = 0;
		foreach (var token in textWithWhiteSpaces)
		{
			if (i != commonTokens.Count && token == commonTokens[i])
			{
				yield return Tuple.Create(token, TokenType.Common);
				++i;
			}
			else
			{
				yield return Tuple.Create(token, TokenType.Specific);
			}
		}
	}
}