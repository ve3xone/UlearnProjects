using System;
using System.Linq;

namespace GaussAlgorithm;

public class Solver
{
    private class Line
    {
        private readonly double[] _data;
        public bool IsUsed { get; private set; }
        public bool IsXSelected { get; private set; }

        public Line(double[] data)
        {
            _data = data;
        }

        public double this[int i]
        {
            get => _data[i];
            set => _data[i] = value;
        }

        private void MarkAsUsed()
        {
            IsUsed = true;
        }

        public void SelectX()
        {
            IsXSelected = true;
        }

        public static void SubtractLines(Line mainLine, Line lineToSubtract, int column)
        {
            if (mainLine == lineToSubtract) return;
            var numbers = -lineToSubtract[column] / mainLine[column];
            for (var i = column; i < mainLine._data.Length; i++)
                lineToSubtract[i] += mainLine[i] * numbers;
            mainLine.MarkAsUsed();
        }
    }

    public double[] Solve(double[][] matrix, double[] freeMembers)
    {
        var numberOfRows = matrix.Length;
        var numberOfColumns = matrix[0].Length;
        var lines = new Line[numberOfRows];
        for (var row = 0; row < numberOfRows; row++)
        {
            var lineData = new double[numberOfColumns + 1];
            Array.Copy(matrix[row], lineData, numberOfColumns);
            lineData[numberOfColumns] = freeMembers[row];
            lines[row] = new Line(lineData);
        }

        for (var col = 0; col < numberOfColumns; col++)
        {
            var mainLine = lines.FirstOrDefault(line => !line.IsUsed && line[col] != 0);
            if (mainLine == null) continue;
            foreach (var line in lines)
            {
                if (line != mainLine)
                    Line.SubtractLines(mainLine, line, col);
            }
        }

        return CalculateSolution(lines, numberOfColumns);
    }

    private static double[] CalculateSolution(Line[] lines, int numberOfColumns)
    {
        var columnsLeft = lines.Count(line => line[numberOfColumns] != 0);
        var solution = new double[numberOfColumns];
        for (var col = 0; col < numberOfColumns && columnsLeft > 0; col++)
        {
            double? x = null;
            foreach (var t in lines)
            {
                if (t[col] == 0 || t.IsXSelected) continue;
                x = t[numberOfColumns] / t[col];
                t.SelectX();
                break;
            }

            solution[col] = x ?? 0;
            if (solution[col] != 0) columnsLeft--;
        }

        if (columnsLeft > 0) throw new NoSolutionException("Система уравнений не может быть решена");
        return solution;
    }
}