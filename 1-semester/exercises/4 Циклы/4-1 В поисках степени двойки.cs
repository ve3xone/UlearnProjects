private static int GetMinPowerOfTwoLargerThan(int number)
{
    int result = 1;
    while (result <= number)
    {
        result *= 2;
    }
    return result;
}