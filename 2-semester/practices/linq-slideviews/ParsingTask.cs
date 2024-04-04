using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class ParsingTask
{
    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
    /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
    /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
    public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
    {
        return lines
            .Skip(1) // Skip header line
            .Select(ParseSlideRecord)
            .Where(record => record != null)
            .ToDictionary(record => record.SlideId);
    }

    private static SlideRecord ParseSlideRecord(string line)
    {
        var parts = line.Split(';');
        if (parts.Length != 3 || !int.TryParse(parts[0], out int slideId))
            return null;

        if (!Enum.TryParse(parts[1], true, out SlideType slideType))
            return null;

        return new SlideRecord(slideId, slideType, parts[2]);
    }

    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
    /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
    /// Такой словарь можно получить методом ParseSlideRecords</param>
    /// <returns>Список информации о посещениях</returns>
    /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
    public static IEnumerable<VisitRecord> ParseVisitRecords(IEnumerable<string> lines, 
                                                             IDictionary<int, SlideRecord> slides)
    {
        return lines
            .Skip(1) // Skip header line
            .Select(line => ParseVisitRecord(line, slides))
            .Where(record => record != null);
    }

    private static VisitRecord ParseVisitRecord(string line, IDictionary<int, SlideRecord> slides)
    {
        var parts = line.Split(';');
        if (parts.Length != 4 ||
            !int.TryParse(parts[0], out int userId) ||
            !int.TryParse(parts[1], out int slideId) ||
            !DateTime.TryParse(parts[2], out var date) || // Parsing date part
            !TimeSpan.TryParse(parts[3], out var time) || // Parsing time part
            !slides.TryGetValue(slideId, out SlideRecord slideRecord))
        {
            throw new FormatException($"Wrong line [{line}]");
        }

        var dateTime = date.Add(time);

        return new VisitRecord(userId, slideId, dateTime, slideRecord.SlideType);
    }
}