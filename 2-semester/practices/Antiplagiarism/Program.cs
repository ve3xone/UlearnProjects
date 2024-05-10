using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antiplagiarism;

class Program
{
	static async Task Main(string[] args)
	{
		Console.WriteLine("Идёт анализ решений...");
		var folder = Folders.SuspiciousSources;
		if (args.Length != 0)
			folder = new DirectoryInfo(args[0]);

		var documents = DocumentLoader.LoadAllStateNames(folder)
			.Select(documentName => new DocumentContent(documentName))
			.ToList();
		var levenshteinCalculator = new LevenshteinCalculator();
		var comparisonResults = LevenshteinCalculator.CompareDocumentsPairwise(documents
				.Select(d => d.Tokens)
				.ToList()
			)
			.OrderBy(GetNormalizedDistance)
			.Take(5);
		Console.WriteLine("Анализ окончен\n Топ-5 самых похожих пар:\n");

		await GenerateReport(documents, comparisonResults);
	}

	private static async Task GenerateReport(List<DocumentContent> documents,
		IEnumerable<ComparisonResult> comparisonResults)
	{
		var isNotImplemented = false;
		await using var stringWriter = new HtmlWriter();

		foreach (var comparisonResult in comparisonResults)
		{
			var first = documents.Find(d => d.Tokens == comparisonResult.Document1);
			var second = documents.Find(d => d.Tokens == comparisonResult.Document2);
			var normalizedDistance = GetNormalizedDistance(comparisonResult);
			Console.WriteLine(
				$"Расширенное расстояние Левенштейна между \"{first.DocumentName}\" и \"{second.DocumentName}\" равно {normalizedDistance}");
			List<string> commonSequence;
			try
			{
				commonSequence = LongestCommonSubsequenceCalculator.Calculate(first.Tokens, second.Tokens);
			}
			catch (NotImplementedException)
			{
				isNotImplemented = true;
				continue;
			}

			await SaveResult(first, second, commonSequence, stringWriter);
		}

		if (isNotImplemented)
		{
			Console.WriteLine("Для генерации отчёта реализуйте класс \"LondestCommonSubsequenceCalculator\"");
		}
		else
		{
			var fileName = Path.Combine(Folders.ComparisonResults.FullName);
			var text = stringWriter.ToString();
			await File.WriteAllTextAsync(fileName, text);
			Console.WriteLine($"Отчёт находится в файле:\n{Folders.ComparisonResults}");
		}
	}

	private static double GetNormalizedDistance(ComparisonResult comparisonResult)
	{
		return 2 * comparisonResult.Distance / (comparisonResult.Document1.Count + comparisonResult.Document2.Count);
	}

	private static async Task SaveResult(DocumentContent first, DocumentContent second, List<string> commonSequence,
		HtmlWriter writer)
	{
		await WriteDocumentsNames(first.DocumentName, second.DocumentName, writer);
		await writer.RenderBeginTag(Tags.Tr);
		await WriteDocumentInHtml(first, commonSequence, writer);
		await WriteDocumentInHtml(second, commonSequence, writer);
		await writer.RenderEndTag();
	}

	private static async Task WriteDocumentsNames(string firstDocumentName, string secondDocumentName,
		HtmlWriter writer)
	{
		await writer.RenderBeginTag(Tags.Tr);

		await writer.RenderBeginTag(Tags.Th);
		await writer.WriteAsync(firstDocumentName);
		await writer.RenderEndTag();

		await writer.RenderBeginTag(Tags.Th);
		await writer.WriteAsync(secondDocumentName);
		await writer.RenderEndTag();

		await writer.RenderEndTag();
	}

	private static async Task WriteDocumentInHtml(DocumentContent document, List<string> commonSequence,
		HtmlWriter writer)
	{
		await writer.RenderBeginTag(Tags.Td);

		var wasCommon = false;
		var whiteSpacesTokens = new StringBuilder();
		foreach (var (token, tag) in document.DevideToCommonAndSpecificTokens(commonSequence))
		{
			if (token.Any(char.IsWhiteSpace))
			{
				whiteSpacesTokens.Append(token);
				continue;
			}

			if (wasCommon && tag == TokenType.Common)
				await WriteWithTag(writer, whiteSpacesTokens.ToString(), TokenType.Common);
			else
				await WriteWithTag(writer, whiteSpacesTokens.ToString(), TokenType.Specific);

			await WriteWithTag(writer, token, tag);
			whiteSpacesTokens.Clear();
			wasCommon = tag == TokenType.Common;
		}

		await WriteWithTag(writer, whiteSpacesTokens.ToString(), TokenType.Specific);

		await writer.RenderEndTag();
	}

	private static async Task WriteWithTag(HtmlWriter writer, string text, TokenType tag)
	{
		if (text.Length == 0)
			return;
		writer.SetClassName(tag.ToString());
		await writer.RenderBeginTag(Tags.Span);
		await writer.WriteAsync(text);
		await writer.RenderEndTag();
	}
}