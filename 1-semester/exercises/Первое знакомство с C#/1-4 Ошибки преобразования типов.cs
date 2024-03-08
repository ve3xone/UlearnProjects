public static void Main()
{
    double pi = Math.PI;
	long tenThousand = 10000L;
	float tenThousandPi = (float)(pi * tenThousand);
	int roundedTenThousandPi = (int)Math.Round(tenThousandPi);
	int integerPartOfTenThousandPi = (int)Math.Floor(tenThousandPi);
	Console.WriteLine(integerPartOfTenThousandPi);
	Console.WriteLine(roundedTenThousandPi);
}