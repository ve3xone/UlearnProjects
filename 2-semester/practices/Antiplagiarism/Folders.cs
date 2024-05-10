using System;
using System.IO;

namespace Antiplagiarism;

public static class Folders
{
	public static readonly DirectoryInfo SuspiciousSources = new(
		Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "SuspiciousSources"));

	public static readonly DirectoryInfo ComparisonResults = new(
		Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "report.html"));
}