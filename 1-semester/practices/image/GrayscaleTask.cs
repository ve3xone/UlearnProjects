namespace Recognizer;

public static class GrayscaleTask
{
    private const double RWeight = 0.299;
    private const double GWeight = 0.587;
    private const double BWeight = 0.114;
    private const double MaxColorValue = 255.0;

    public static double[,] ToGrayscale(Pixel[,] original)
    {
        int width = original.GetLength(0);
        int height = original.GetLength(1);
        double[,] grayscale = new double[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pixel = original[x, y];

                grayscale[x, y] = (RWeight * pixel.R + GWeight * pixel.G + BWeight * pixel.B) / MaxColorValue;
            }
        }

        return grayscale;
    }
}