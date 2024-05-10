using System;
using System.Collections.Generic;

namespace Antiplagiarism;

public static class LongestCommonSubsequenceCalculator
{
	public static List<string> Calculate(List<string> first, List<string> second)
	{
		var opt = CreateOptimizationTable(first, second);
		return RestoreAnswer(opt, first, second);
	}

	private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
	{
		throw new NotImplementedException();
	}

	private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
	{
		throw new NotImplementedException();
	}
}