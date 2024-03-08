public static bool ContainsAtIndex(int[] array, int[] subArray, int startIndex)
{
    for (var i = 0; i < subArray.Length; i++)
        if (array[startIndex + i] != subArray[i])
            return false;
    return true;
}