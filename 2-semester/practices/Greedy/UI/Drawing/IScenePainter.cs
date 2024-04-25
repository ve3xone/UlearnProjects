using Avalonia;
using Avalonia.Media;

namespace Greedy.UI.Drawing;

public interface IScenePainter
{
	Size RealSize { get; }
	Size CellSize { get; }
	void Paint(DrawingContext drawingContext, double zoomScale);
}