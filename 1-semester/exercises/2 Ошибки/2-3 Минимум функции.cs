private static string GetMinX(int a, int b, int c)
{
	// Код выполниться, если или первое, или второе условие
    // окажется верным
    if (a > 0 || b == 0)
    {
        double x = (double) -b / (2 * a);
        return x.ToString();
    }
    else
    {
        return "Impossible";
    }
}