public static int[] GetPoweredArray(int[] arr, int power)
{
    // Создаем новый массив той же длины, что и входной массив
    int[] poweredArray = new int[arr.Length];

    // Проходим по каждому элементу входного массива и возводим его в заданную степень
    for (int i = 0; i < arr.Length; i++)
    {
        // Возводим элемент в заданную степень и записываем результат в новый массив
        poweredArray[i] = (int)Math.Pow(arr[i], power);
    }
    
    // Возвращаем новый массив с результатами операции
    return poweredArray;
}