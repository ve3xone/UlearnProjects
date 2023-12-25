using System;
using System.Collections.Generic;

namespace Autocomplete
{
    public class LeftBorderTask
    {
        public static int GetLeftBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            if (left >= right - 1)
                return left;

            int mid = left + (right - left) / 2;
            int comparisonRes = string.Compare(prefix, phrases[mid], StringComparison.InvariantCultureIgnoreCase);

			if (comparisonRes < 0 || comparisonRes == 0 
				&& phrases[mid].StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                return GetLeftBorderIndex(phrases, prefix, left, mid);
            
            return GetLeftBorderIndex(phrases, prefix, mid, right);
        }
    }
}