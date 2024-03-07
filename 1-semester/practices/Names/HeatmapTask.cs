namespace Names;

internal static class HeatmapTask
{
    public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
    {
        var days = GetLabels(2, 31);
        var mounts = GetLabels(1, 12);

        double[,] values = GetValues(names);

        return new HeatmapData("Пример карты интерсивностей",
                               values, days, mounts);
    }

    private static string[] GetLabels(int min, int max)
    {
        var labels = new string[max - min + 1];
        for (var i =0; i < labels.Length; i++)
            labels[i] = (i + min).ToString();
        return labels;
    }

    private static double[,] GetValues(NameData[] names)
    {
        double[,] values = new double[30, 12];
        foreach (var name in names)
        {
            var day = name.BirthDate.Day;
            var month = name.BirthDate.Month;
            if (day != 1)
                values[day - 2, month - 1]++;
        }
        return values;
    }
}