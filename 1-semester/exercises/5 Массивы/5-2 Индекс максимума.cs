public static int MaxIndex(double[] array)
{
    if (array.Length == 0)
        // Если массив пуст, возвращаем -1
        return -1;

    double max = array[0]; // Первый элемент массива принимаем за максимальный
    int maxIndex = 0; // Индекс максимального элемента

    for (int i = 1; i < array.Length; i++)
    {
		if (array[i] > max)
		{
            // Если текущий элемент больше максимального, обновляем максимальный элемент и его индекс
            max = array[i];
			maxIndex = i;
		}
        else if (array[i] == max && i < maxIndex)
            // Если текущий элемент равен максимальному, проверяем индекс
            // Если текущий индекс меньше сохраненного, обновляем индекс максимального элемента
            maxIndex = i;
    }
    return maxIndex;
}