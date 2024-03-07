namespace Names
{
    internal static class HistogramTask
    {
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            var numberOfDays = 31;
            var daysNumbers = GetDaysNumbers(numberOfDays);

            var birthsCounts = GetBirthsCounts(numberOfDays, names, name);

            return new HistogramData(string.Format("Рождаемость людей с именем '{0}'", name),
                                     daysNumbers, birthsCounts);
        }

        private static string[] GetDaysNumbers(int numberOfDays)
        {
            var daysNumbers = new string[numberOfDays];
            for (int i = 0; i < numberOfDays; i++)
                daysNumbers[i] = (i + 1).ToString();
            return daysNumbers;
        }

        private static double[] GetBirthsCounts(int numberOfDays, NameData[] names, string name)
        {
            var birthsCounts = new double[numberOfDays];
            foreach (var person in names)
            {
                if (person.Name == name && person.BirthDate.Day != 1)
                    birthsCounts[person.BirthDate.Day - 1]++;
            }
            return birthsCounts;
        }
    }
}