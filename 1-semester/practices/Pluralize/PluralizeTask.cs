namespace Pluralize;

public static class PluralizeTask
{
    public static string PluralizeRubles(int count)
    {
        if (count % 10 == 1 && count % 100 != 11)
            return "рубль";
        else if ((count % 100 < 10 || count % 100 >= 20) &&
                  count % 10 >= 2 && count % 10 <= 4)
            return "рубля";
        else
            return "рублей";
    }
}