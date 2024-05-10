using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Antiplagiarism;

public class DocumentLoader
{
	public static IEnumerable<string> LoadStateFromFolder(DirectoryInfo folder, string documentName)
	{
		var documentFile = folder.GetFiles($"{documentName}.cs", SearchOption.AllDirectories).Single();
		var documentText = File.ReadAllText(documentFile.FullName);
		return Tokenizer.Tokenize(documentText);
	}

	public static IEnumerable<string> LoadAllStateNames(DirectoryInfo folder)
	{
		return folder.GetFiles("*", SearchOption.AllDirectories)
			.Where(info => info.Name.EndsWith(".cs"))
			.Select(file => file.FullName);
	}
}