static T Max<T>(T[] source)
{
    if(source.Length == 0)
        return default(T);
    return source.Max();
}