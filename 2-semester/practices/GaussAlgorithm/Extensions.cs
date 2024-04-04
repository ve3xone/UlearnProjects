using System;
using System.Linq;

namespace GaussAlgorithm;

public static class Extensions
{
	public static string FormatMatrix(this double[][] matrix)
	{
		return string.Join(Environment.NewLine, matrix.Select(row => string.Join("\t", row)));
	}
}