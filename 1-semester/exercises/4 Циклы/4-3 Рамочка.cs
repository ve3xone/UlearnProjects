private static void WriteTextWithBorder(string text)
{
    int length = text.Length + 2; // Длина текста плюс два пробела слева и справа, плюс по два символа + и -
    Console.WriteLine("+" + new string('-', length) + "+"); // Верхняя горизонтальная линия
    Console.WriteLine("| " + text + " |"); // Текст внутри рамки
    Console.WriteLine("+" + new string('-', length) + "+"); // Нижняя горизонтальная линия
}