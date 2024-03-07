public static double Calculate(string userInput)
{
    string[] values = userInput.Split(' ');

    var sum = double.Parse(values[0]);
    var rate = double.Parse(values[1]) / 100; // Процентная ставка в десятичных долях
    var months = int.Parse(values[2]);

    // Количество периодов капитализации в году (месяцев в году)
    int compoundingFrequency = 12;

    double accumulatedAmount = sum * Math.Pow(1 + rate / compoundingFrequency,
                                              compoundingFrequency * months / compoundingFrequency);
    return accumulatedAmount;
}