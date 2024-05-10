using System;
using System.Collections.Generic;
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism;

public class LevenshteinCalculator
{
    // Для ulearn нужно уберать у всех static
    public static List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
    {
        var comparisonResults = new List<ComparisonResult>();
        var documentCount = documents.Count;

        for (var i = 0; i < documentCount; i++)
        {
            for (var j = i + 1; j < documentCount; j++)
            {
                var distance = ComputeLevenshteinDistance(documents[i], documents[j]);
                var result = new ComparisonResult(documents[i], documents[j], distance);
                comparisonResults.Add(result);
            }
        }

        return comparisonResults;
    }

    private static double ComputeLevenshteinDistance(DocumentTokens first, DocumentTokens second)
    {
        var previousOpt = InitializePreviousOpt(second.Count);
        var currentOpt = new double[second.Count + 1];

        for (var i = 1; i <= first.Count; ++i)
        {
            currentOpt[0] = i;

            CalculateCurrentOpt(first, second, previousOpt, currentOpt, i);

            Array.Copy(currentOpt, previousOpt, second.Count + 1);
        }

        return currentOpt[second.Count];
    }

    private static double[] InitializePreviousOpt(int length)
    {
        var opt = new double[length + 1];
        for (var i = 0; i <= length; ++i)
            opt[i] = i;
        return opt;
    }

    private static void CalculateCurrentOpt(DocumentTokens first, DocumentTokens second,
                                     double[] previousOpt, double[] currentOpt, int i)
    {
        for (var j = 1; j <= second.Count; ++j)
        {
            if (first[i - 1] == second[j - 1])
                currentOpt[j] = previousOpt[j - 1];
            else
                currentOpt[j] = CalculateMinimumDistance(first[i - 1], second[j - 1],
                                                         previousOpt, currentOpt, j);
        }
    }

    private static double CalculateMinimumDistance(string token1, string token2,
                                            double[] previousOpt, double[] currentOpt, int j)
    {
        return Math.Min(1 + previousOpt[j],
                        TokenDistanceCalculator.GetTokenDistance(token1, token2) +
                        Math.Min(previousOpt[j - 1], currentOpt[j - 1]));
    }
}