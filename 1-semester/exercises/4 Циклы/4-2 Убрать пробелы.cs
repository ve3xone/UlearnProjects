public static string RemoveStartSpaces(string text)
{
    int startIndex = 0;
    while (startIndex < text.Length && char.IsWhiteSpace(text[startIndex]))
    {
        startIndex++;
    }
    return text.Substring(startIndex);
}