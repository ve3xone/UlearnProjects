using System;
using System.Text;

namespace GaussAlgorithm;

public class NoSolutionException : Exception
{
	public NoSolutionException(string message) : base(message)
	{ }

	public NoSolutionException(double[][] initialMatrix, double[] freeMembers, double[][] matrixAfterSolve)
		: base(GetMessage(initialMatrix, freeMembers, matrixAfterSolve))
	{ }

	private static string GetMessage(double[][] sourceMatrix, double[] freeMembers, double[][] solvedMatrix)
	{
		var builder = new StringBuilder();
		builder.Append("Initial matrix:" + Environment.NewLine + sourceMatrix.FormatMatrix() + Environment.NewLine);
		builder.Append("Free members: [" + string.Join(", ", freeMembers) + "]" + Environment.NewLine);
		builder.Append("Matrix after Solve:" + Environment.NewLine + solvedMatrix.FormatMatrix());
		return builder.ToString();
	}
}