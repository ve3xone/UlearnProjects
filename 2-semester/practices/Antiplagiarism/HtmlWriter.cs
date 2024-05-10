using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Antiplagiarism;

public class HtmlWriter : StringWriter
{
	private string className;
	private readonly Stack<Tags> tagsStack = new();

	private const string HtmlBegin =
		@"<!DOCTYPE html>
<html>
<head>
<meta charset=""utf-8""/>
<title>Похожие решения</title>
<style type=""text/css"">
* {padding-top:0;margin-top:0;border-top:0;}
.Common {background-color: #ff9999;}
tr, td {vertical-align: top; border-left: 1px solid #ddd;border-bottom: 1px solid #ddd;}
th {background-color: #ddd} 
</style>
</head>
<body>
<pre>
<table valign=""top"">";

	private const string HtmlEnd = @"</table></pre></body></html>";

	public HtmlWriter()
	{
		WriteLine(HtmlBegin);
	}

	public override string ToString()
	{
		WriteLine(HtmlEnd);
		return base.ToString();
	}

	public async Task RenderBeginTag(Tags tag)
	{
		tagsStack.Push(tag);

		await RenderTag(tag);
	}

	public async Task RenderEndTag()
	{
		if (tagsStack.Count == 0)
			throw new InvalidOperationException();

		var tag = tagsStack.Pop();
		await RenderTag(tag, true);
		SetClassName(null);
	}

	public void SetClassName(string className)
	{
		this.className = className;
	}

	private async Task RenderTag(Tags tag, bool close = false)
	{
		var prefix = "";
		var postfix = "";

		var content = tag switch
		{
			Tags.Tr => "tr",
			Tags.Td => "td",
			Tags.Th => "th",
			Tags.Span => "span",
			_ => throw new ArgumentOutOfRangeException(nameof(tag), tag, null)
		};

		if (close)
			prefix = "/";
		else if (className != null)
			postfix = $" class=\"{className}\"";

		await WriteAsync($"<{prefix}{content}{postfix}>");
	}
}

public enum Tags
{
	Tr,
	Td,
	Th,
	Span,
}