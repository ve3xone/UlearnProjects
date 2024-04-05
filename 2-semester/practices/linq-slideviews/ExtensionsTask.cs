using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
    /// <summary>
    /// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
    /// Медиана списка из четного количества элементов — это среднее арифметическое
    /// двух серединных элементов списка после сортировки.
    /// </summary>
    /// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
    public static double Median(this IEnumerable<double> items)
    {
        if (items == null)
            throw new InvalidOperationException();

        var sortedItems = items.OrderBy(x => x).ToList();
        var middleIndex = sortedItems.Count / 2;

        if (sortedItems.Count == 0)
            throw new InvalidOperationException();
        else if (sortedItems.Count % 2 == 1)
            return sortedItems[middleIndex];
        else
            return (sortedItems[middleIndex - 1] + sortedItems[middleIndex]) / 2;
    }

    /// <returns>
    /// Возвращает последовательность, состоящую из пар соседних элементов.
    /// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
    /// </returns>
    public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
    {
        bool firstItem = true;
        T prevItem = default;

        foreach (var currentItem in items)
        {
            if (!firstItem)
            {
                yield return (prevItem, currentItem);
            }

            firstItem = false;
            prevItem = currentItem;
        }
    }
}