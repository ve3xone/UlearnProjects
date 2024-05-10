using System;
using System.Collections.Generic;
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism;

public class LevenshteinCalculator
{
    //Для ulearn нужно убрать на всех методах static
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
        var previousOptions = InitializePreviousOptions(second.Count);
        var currentOptions = new double[second.Count + 1];

        for (var i = 1; i <= first.Count; ++i)
        {
            currentOptions[0] = i;

            CalculateCurrentOptions(first, second, previousOptions, currentOptions, i);

            Array.Copy(currentOptions, previousOptions, second.Count + 1);
        }

        return currentOptions[second.Count];
    }

    private static double[] InitializePreviousOptions(int length)
    {
        var options = new double[length + 1];
        for (var i = 0; i <= length; ++i)
            options[i] = i;
        return options;
    }

    private static void CalculateCurrentOptions(DocumentTokens first, DocumentTokens second,
                                                double[] previousOptions, double[] currentOptions, int i)
    {
        for (var j = 1; j <= second.Count; ++j)
        {
            if (first[i - 1] == second[j - 1])
                currentOptions[j] = previousOptions[j - 1];
            else
                currentOptions[j] = CalculateMinimumDistance(first[i - 1], second[j - 1],
                                                             previousOptions, currentOptions, j);
        }
    }

    private static double CalculateMinimumDistance(string token1, string token2,
                                                   double[] previousOptions, double[] currentOptions, int j)
    {
        return Math.Min(1 + previousOptions[j],
                        TokenDistanceCalculator.GetTokenDistance(token1, token2) +
                        Math.Min(previousOptions[j - 1], currentOptions[j - 1]));
    }
}