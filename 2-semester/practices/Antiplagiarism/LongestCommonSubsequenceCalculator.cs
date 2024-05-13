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
        int f = first.Count;
        int s = second.Count;
        int[,] opt = new int[f + 1, s + 1];

        for (int i = 1; i <= f; i++)
        {
            for (int j = 1; j <= s; j++)
            {
                opt[i, j] = first[i - 1] == second[j - 1] ? opt[i - 1, j - 1] + 1 :
                                                            Math.Max(opt[i - 1, j], opt[i, j - 1]);
            }
        }

        return opt;
    }

    private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
    {
        int f = first.Count;
        int s = second.Count;
        List<string> result = new();

        while (f > 0 && s > 0)
        {
            if (first[f - 1] == second[s - 1])
            {
                result.Insert(0, first[f - 1]);
                f--;
                s--;
            }
            else if (opt[f - 1, s] > opt[f, s - 1])
            {
                f--;
            }
            else
            {
                s--;
            }
        }

        return result;
    }
}