using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Manipulation.UI;

public class Frame : UserControl
{
    internal MainWindow Window;

    public override void Render(DrawingContext context)
    {
        var shoulderPos = new Point(Window.ClientSize.Width / 2f, Window.ClientSize.Height / 2f);
        VisualizerTask.DrawManipulator(context, shoulderPos);
        base.Render(context);
    }
}