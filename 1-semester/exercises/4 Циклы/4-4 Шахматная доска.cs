private static void WriteBoard(int size)
{
    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            // Если сумма индексов строки и столбца четная, то клетка черная (шарп),
            // в противном случае клетка белая (точка)
            if ((i + j) % 2 == 0)
            {
                Console.Write("#");
            }
            else
            {
                Console.Write(".");
            }
        }
        Console.WriteLine(); // Переход на новую строку после заполнения одной строки доски
    }
    Console.WriteLine(); // Пустая строка между досками разных размеров
}