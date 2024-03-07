public static double Calculate(string userInput)
{
    string[] values = userInput.Split(' ');

    var sum = double.Parse(values[0]);
    var rate = double.Parse(values[1]) / 100; // ���������� ������ � ���������� �����
    var months = int.Parse(values[2]);

    // ���������� �������� ������������� � ���� (������� � ����)
    int compoundingFrequency = 12;

    double accumulatedAmount = sum * Math.Pow(1 + rate / compoundingFrequency,
                                              compoundingFrequency * months / compoundingFrequency);
    return accumulatedAmount;
}