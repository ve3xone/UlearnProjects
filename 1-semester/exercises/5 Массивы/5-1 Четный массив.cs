static int[] GetFirstEvenNumbers(int count)
{
    int[] evenNumbers = new int[count];
    int num = 2; // Первое четное число
    
    for (int i = 0; i < count; i++)
    {
        evenNumbers[i] = num;
        num += 2; // Увеличиваем на 2 для следующего четного числа
    }
    
    return evenNumbers;
}