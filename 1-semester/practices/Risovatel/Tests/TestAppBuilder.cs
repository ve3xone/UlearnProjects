using Avalonia;
using Avalonia.Headless;
using RefactorMe.Tests;
using RefactorMe.UI;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]

namespace RefactorMe.Tests;

public class TestAppBuilder
{
	public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
		.UseSkia()
		.UseHeadless(new AvaloniaHeadlessPlatformOptions
		{
			UseHeadlessDrawing = false
		});
}