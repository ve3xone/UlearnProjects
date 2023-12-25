using System;
using System.Collections.Generic;

namespace Autocomplete
{
    public class RightBorderTask
    {
        public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            var comparer = StringComparer.Create(System.Globalization.CultureInfo.CurrentCulture, true);

            while (left + 1 != right)
            {
                int middle = (left + right) / 2;
                if (comparer.Compare(phrases[middle], prefix) > 0
                    && !phrases[middle].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    right = middle;
                }
				else
				{
					left = middle;
				}
            }
            return right;
        }
    }
}