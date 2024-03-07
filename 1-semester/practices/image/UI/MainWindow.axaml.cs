using System;
using System.Drawing;
using System.Drawing.Imaging;
using Avalonia.Controls;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Image = System.Drawing.Image;

namespace Recognizer.UI;

public static class ImageExtensions
{
	public static Bitmap? ConvertToAvaloniaBitmap(this Image? bitmap)
	{
		if (bitmap == null)
			return null;

		var bitmapTmp = new System.Drawing.Bitmap(bitmap);
		var bitmapData = bitmapTmp.LockBits(new Rectangle(0, 0, bitmapTmp.Width, bitmapTmp.Height),
			ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		var bitmapNew = new Bitmap(Avalonia.Platform.PixelFormat.Bgra8888, Avalonia.Platform.AlphaFormat.Premul,
			bitmapData.Scan0,
			new Avalonia.PixelSize(bitmapData.Width, bitmapData.Height),
			new Avalonia.Vector(96, 96),
			bitmapData.Stride);
		bitmapTmp.UnlockBits(bitmapData);
		bitmapTmp.Dispose();

		return bitmapNew;
	}
}

public partial class MainWindow : Window
{
	private const int ResizeRate = 2;

	public MainWindow()
	{
		InitializeComponent();
		var image = this.Find<Avalonia.Controls.Image>("Default");
		var pixelsImage = this.Find<Avalonia.Controls.Image>("Pixels");
		var grayscaleImage = this.Find<Avalonia.Controls.Image>("Grayscale");
		var clearImage = this.Find<Avalonia.Controls.Image>("Clear");
		var sobellImage = this.Find<Avalonia.Controls.Image>("Sobell");
		var trashholdImage = this.Find<Avalonia.Controls.Image>("Trashhold");

		var bmp = (System.Drawing.Bitmap)Image.FromFile("eurobot.bmp");
		image.Source = bmp.ConvertToAvaloniaBitmap();

		var pixels = LoadPixels(bmp);
		pixelsImage.Source = ConvertToBitmap(pixels).ConvertToAvaloniaBitmap();

		var grayscale = GrayscaleTask.ToGrayscale(pixels);
		grayscaleImage.Source = ConvertToBitmap(grayscale).ConvertToAvaloniaBitmap();
		
		var clear = MedianFilterTask.MedianFilter(grayscale);
		clearImage.Source = ConvertToBitmap(clear).ConvertToAvaloniaBitmap();
		
		var sobell = SobelFilterTask.SobelFilter(clear, new double[,] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } });
		sobellImage.Source = ConvertToBitmap(sobell).ConvertToAvaloniaBitmap();
		
		var trashhold = ThresholdFilterTask.ThresholdFilter(sobell, 0.1);
		trashholdImage.Source = ConvertToBitmap(trashhold).ConvertToAvaloniaBitmap();
	}

	public static Pixel[,] LoadPixels(System.Drawing.Bitmap bmp)
	{
		var pixels = new Pixel[bmp.Width, bmp.Height];
		for (var x = 0; x < bmp.Width; x++)
		for (var y = 0; y < bmp.Height; y++)
			pixels[x, y] = new Pixel(bmp.GetPixel(x, y));
		return pixels;
	}

	private static System.Drawing.Bitmap ConvertToBitmap(int width, int height, Func<int, int, Color> getPixelColor)
	{
		var bmp = new System.Drawing.Bitmap(ResizeRate * width, ResizeRate * height);
		using var g = Graphics.FromImage(bmp);
		for (var x = 0; x < width; x++)
		for (var y = 0; y < height; y++)
			g.FillRectangle(new SolidBrush(getPixelColor(x, y)),
				ResizeRate * x,
				ResizeRate * y,
				ResizeRate,
				ResizeRate
			);

		return bmp;
	}

	private static System.Drawing.Bitmap ConvertToBitmap(Pixel[,] array)
	{
		return ConvertToBitmap(array.GetLength(0), array.GetLength(1),
			(x, y) => Color.FromArgb(array[x, y].R, array[x, y].G, array[x, y].B));
	}

	private static System.Drawing.Bitmap ConvertToBitmap(double[,] array)
	{
		return ConvertToBitmap(array.GetLength(0), array.GetLength(1), (x, y) =>
		{
			var gray = (int)(255 * array[x, y]);
			gray = Math.Min(gray, 255);
			gray = Math.Max(gray, 0);
			return Color.FromArgb(gray, gray, gray);
		});
	}
}