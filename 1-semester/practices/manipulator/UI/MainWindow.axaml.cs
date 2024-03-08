using Avalonia.Controls;
using Avalonia.Media;

namespace Manipulation.UI;

public partial class MainWindow : Window, IDisplay
{
	public MainWindow()
	{
		InitializeComponent();
		Frame.Window = this;
		Background = new SolidColorBrush(new Color(255, 255, 230, 230));

		KeyDown += (_, ev) => VisualizerTask.KeyDown(this, ev);
		PointerMoved += (_, ev) => VisualizerTask.MouseMove(this, ev);
		PointerWheelChanged += (_, ev) => VisualizerTask.MouseWheel(this, ev);
	}

	public new void InvalidateVisual() => Frame.InvalidateVisual();
}