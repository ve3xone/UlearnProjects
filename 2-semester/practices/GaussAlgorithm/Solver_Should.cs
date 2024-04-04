using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace GaussAlgorithm;

[TestFixture]
public class Solver_Should
{
	private Solver solver;
	private readonly Random random = new Random(764524336);

	[SetUp]
	public void SetUp()
	{
		solver = new Solver();
	}

	[TestCase(1, 1, 42)]
	[TestCase(1, 2, -42)]
	[TestCase(1, -100, 0)]
	[TestCase(1, 0, 0)]
	[TestCase(1,
		1, 21,
		2, 42)]
	[TestCase(1,
		2, 42,
		1, 21)]
	[TestCase(1,
		0, 0,
		0, 0)]
	public void SolveCommonCases_1x1(int columnCount, params double[] values) 
		=> AssertIsSolvable(columnCount, values);

	[TestCase(2, 1, 2, 42)]
	[TestCase(2, 0, -2, 42)]
	[TestCase(2, -1, 0, 42)]
	[TestCase(2, 0, 0, 0)]
	[TestCase(2,
		1, 2, 1,
		3, -1, 2)]
	[TestCase(2,
		1, -2, 42,
		2, 1, -31)]
	[TestCase(2,
		-1, -2, -42,
		2, 4, 84)]
	[TestCase(2,
		1, 2, 42,
		-2, -4, -84)]
	public void SolveCommonCases_2x2(int columnCount, params double[] values)
		=> AssertIsSolvable(columnCount, values);

	[TestCase(3,
		2, 4, 6, 12,
		1, 2, 3, 6,
		1, 1, 3, 5)]
	[TestCase(3,
		2, 4, 6, 12,
		1, 2, 4, 7,
		1, 2, 6, 9)]
	[TestCase(3,
		1.0, 1.0, 3.0, 5,
		1.0, 3.0, 1.0, 5,
		1.0, 2.0, 6.0, 9,
		2.0, 4.0, 12.0, 18)]
	public void SolveCommonCases_3x3(int columnCount, params double[] values)
		=> AssertIsSolvable(columnCount, values);

	[TestCase(2, 2)]
	[TestCase(3, 3)]
	[TestCase(4, 4)]
	[TestCase(5, 5)] 
	[TestCase(10, 10)]
	[TestCase(3, 5)]
	[Repeat(500)]
	public void SolveRandomMatrix(int rows, int columns)
	{
		var randomMatrix = CreateRandomMatrix(rows, columns);
		var randomFreeMembers = GetRandomFreeMembers(randomMatrix);
		var actualSolution = solver.Solve(randomMatrix, randomFreeMembers);
		AssertIsSolution(actualSolution, randomMatrix, randomFreeMembers, 1e-3);
	}

	[TestCase(1,
		1, 1,
		1, -1)]
	[TestCase(2,
		1, 2, 42,
		2, 4, 42)]
	[TestCase(3,
		2, 4, 6, 12,
		1, 2, 3, 6,
		1, 2, 3, 7,
		1, 1, 3, 5)]
	public void ThrowsException_WhenNoSolution(int columnCount, params double[] args)
	{
		var (matrix, freeMembers) = BuildSystem(columnCount, args);
		Assert.Throws<NoSolutionException>(() => solver.Solve(matrix, freeMembers));
	}

	[TestCase(10)]
	[TestCase(5)]
	[TestCase(4)]
	[TestCase(3)]
	[TestCase(2)]
	public void ThrowsException_WhenNoSolutionOnRandomMatrix(int matrixSize)
	{
		var (matrix, freeMembers) = CreateRandomSystemWithLinearlyDependentRows(matrixSize, false);
		Assert.Throws<NoSolutionException>(() => solver.Solve(matrix, freeMembers));
	}

	[TestCase(10)]
	[TestCase(5)]
	[TestCase(4)]
	[TestCase(3)]
	[TestCase(2)]
	public void Solve_WhenHasLinearlyDependentRows(int matrixSize)
	{
		var (matrix, freeMembers) = CreateRandomSystemWithLinearlyDependentRows(matrixSize, true);
		var actualSolution = solver.Solve(matrix, freeMembers);
		AssertIsSolution(actualSolution, matrix, freeMembers, 1e-3);
	}

	private void AssertIsSolvable(int columnCount, double[] values)
	{
		var (matrix, freeMembers) = BuildSystem(columnCount, values);
		var actualSolution = solver.Solve(matrix, freeMembers);
		AssertIsSolution(actualSolution, matrix, freeMembers, 1e-3);
	}

	private static (double[][] matrix, double[] freeMembers) BuildSystem(int columnCount, double[] values)
	{
		var matrix = new List<double[]>();
		var freeMembers = new List<double>();
		var line = new List<double>();
		for (var i = 0; i < values.Length; i++)
		{
			if ((i + 1) % (columnCount + 1) == 0)
			{
				matrix.Add(line.ToArray());
				line = new List<double>();
				freeMembers.Add(values[i]);
			}
			else
				line.Add(values[i]);
		}

		return (matrix.ToArray(), freeMembers.ToArray());
	}

	private double[] GetRandomFreeMembers(double[][] matrix)
	{
		const int max = 100;
		var xs = Enumerable.Range(0, matrix[0].Length).Select(c => random.Next(-max, max));
		var freeMembers = matrix.Select(row => row.Zip(xs, (f, s) => f * s).Sum()).ToArray();
		return freeMembers;
	}

	private double[][] CreateRandomMatrix(int rows, int columns, int valuesRange = 1000)
	{
		return Enumerable.Range(0, rows).Select(r => CreateRandomRow(columns, valuesRange)).ToArray();
	}

	private double[] CreateRandomRow(int columns, int maxValue)
	{
		double[] row;
		do
		{
			row = Enumerable.Range(0, columns).Select(c => (double) random.Next(-maxValue, maxValue)).ToArray();
		} while (row.All(x => Math.Abs(x) < 1e-3));
		return row;
	}

	private (double[][] matrix, double[] freeMembers) CreateRandomSystemWithLinearlyDependentRows(int matrixSize, bool isSolvable)
	{
		var matrix = CreateRandomMatrix(matrixSize, matrixSize);
		var freeMembers = GetRandomFreeMembers(matrix);
		var iRow = random.Next(matrixSize);
		int jRow;
		do
		{
			jRow = random.Next(matrixSize);
		} while (iRow == jRow);
		matrix[iRow] = matrix[jRow].ToArray();
		freeMembers[iRow] = isSolvable ? freeMembers[jRow] : freeMembers[jRow] + 1;
		return (matrix, freeMembers);
	}

	private static void AssertIsSolution(double[] actual, double[][] matrix, double[] freeMembers, double accuracy)
	{
		for (var i = 0; i < matrix.GetLength(0); i++)
		{
			var line = matrix[i];
			var result = line.Select((element, j) => element * actual[j]).Sum();
			Assert.AreEqual(freeMembers[i], result, accuracy, GetAssertionMessage(matrix, freeMembers, actual, i+1));
		}
	}

	private static string GetAssertionMessage(
		double[][] sourceMatrix, double[] freeMembers,
		double[] actualSolution, int equationIndex)
	{
		var builder = new StringBuilder("The solution is incorrect!" + Environment.NewLine + Environment.NewLine);
		builder.Append("Initial matrix:" + Environment.NewLine + sourceMatrix.FormatMatrix() + Environment.NewLine);
		builder.Append("Free members: [" + string.Join(", ", freeMembers) + "]" + Environment.NewLine);
		builder.Append("Your solution: [" + string.Join(", ", actualSolution) + "]" + Environment.NewLine);
		builder.Append($"Equation #{equationIndex} is not satisfied");
		return builder.ToString();
	}
}