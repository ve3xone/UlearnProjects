using Avalonia.VisualTree;

namespace Manipulation;

public interface IDisplay : IVisual
{
	void InvalidateVisual();
}