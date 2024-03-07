using System.IO;
using Avalonia.Headless;
using Avalonia.Headless.NUnit;
using Avalonia.Media.Imaging;
using NUnit.Framework;
using RefactorMe.UI;

namespace RefactorMe.Tests;

[TestFixture]
public class DrawingProgram_Tests
{
	//24 bits RGB, 800x600
	private const string expectedBmpPath = "expected.bmp";

	[AvaloniaTest]
	public void DrawExpectedImage()
	{
		var expectedBmp = new Bitmap(expectedBmpPath);
		using var expected = new MemoryStream();
		using var actual = new MemoryStream();

		var window = new MainWindow();
		window.Show();
		expectedBmp.Save(expected);
		window.CaptureRenderedFrame()?.Save(actual);

		Assert.IsTrue(AreStreamsEqual(expected, actual));
	}

	private static bool AreStreamsEqual(Stream expected, Stream actual)
	{
		if (expected.Length != actual.Length)
			return false;

		expected.Position = 0;
		actual.Position = 0;

		for (var i = 0; i < expected.Length; i++)
		{
			var aByte = expected.ReadByte();
			var bByte = actual.ReadByte();
			if (aByte.CompareTo(bByte) != 0)
				return false;
		}

		return true;
	}
}