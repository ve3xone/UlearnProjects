using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Transformation;
using Size = Avalonia.Size;

namespace Greedy.UI.Drawing;

public class GreedyCanvas : Canvas
{
	public IScenePainter painter;
	private Avalonia.Point centerLogicalPos;
	private bool dragInProgress;
	private Avalonia.Point dragStart;
	private Avalonia.Point mouseLogicalPos;
	private ITransform? transform;
	private double zoomScale;

	public Avalonia.Point CenterLogicalPos
	{
		get => centerLogicalPos;
		set
		{
			centerLogicalPos = value;
			UpdateScale();
		}
	}

	public double ZoomScale
	{
		get => zoomScale;
		set
		{
			var dx = (Bounds.Width / value - Bounds.Width / zoomScale) / 2f;
			var dy = (Bounds.Height / value - Bounds.Height / zoomScale) / 2f;
			var oldScale = zoomScale;
			zoomScale = Math.Min(4f, Math.Max(0.05f, value));
			if (Math.Abs(oldScale - zoomScale) > 0.001f)
				CenterLogicalPos += new Avalonia.Point(dx, dy);
		}
	}

	public double BaseScale { get; set; }

	public void UpdateScale()
	{
		var realRatio = BaseScale + ZoomScale - BaseScale;
		var matrix = MatrixHelper.ScaleAndTranslate(
			ZoomScale,
			ZoomScale,
			CenterLogicalPos.X * realRatio,
			CenterLogicalPos.Y * realRatio);
		var transformBuilder = new TransformOperations.Builder(1);
		transformBuilder.AppendMatrix(matrix);
		RenderTransform = transformBuilder.Build();
	}

	public void ResetScale()
	{
		var sceneSize = painter.RealSize;
		var scaleForWidth = Width / sceneSize.Width;
		var scaleForHeight = Height / sceneSize.Height;
		var newZoomScale = scaleForWidth < scaleForHeight ? scaleForWidth : scaleForHeight;

		var dw = Width / newZoomScale - sceneSize.Width;
		var dh = Height / newZoomScale - sceneSize.Height;

		BaseScale = newZoomScale;
		zoomScale = newZoomScale;
		CenterLogicalPos = new Avalonia.Point(dw / 2f, dh / 2f);
	}

	protected override void OnPointerPressed(PointerPressedEventArgs e)
	{
		var props = e.GetCurrentPoint(this).Properties;
		var position = e.GetPosition(this);

		switch (props.PointerUpdateKind)
		{
			case PointerUpdateKind.LeftButtonPressed:
				ShowCoords(position);
				break;
			case PointerUpdateKind.MiddleButtonPressed:
				ResetScale();
				break;
			case PointerUpdateKind.RightButtonPressed:
				dragInProgress = true;
				dragStart = position;
				break;
		}
	}

	protected override void OnPointerReleased(PointerReleasedEventArgs e)
	{
		var props = e.GetCurrentPoint(this).Properties;

		dragInProgress = props.PointerUpdateKind switch
		{
			PointerUpdateKind.RightButtonReleased => false,
			_ => dragInProgress
		};
	}

	private Task showCoordsTask;

	private async Task ShowCoords(Avalonia.Point position, int displayTimeout = 3000)
	{
		mouseLogicalPos = position;
		InvalidateVisual();
		var delay = Task.Delay(displayTimeout);
		showCoordsTask = delay;
		await showCoordsTask;
		if (delay != showCoordsTask) return;
		mouseLogicalPos = new Avalonia.Point(0, 0);
		InvalidateVisual();
	}

	protected override void OnPointerMoved(PointerEventArgs e)
	{
		var position = e.GetPosition(this);

		if (!dragInProgress) return;
		var dx = position.X - dragStart.X;
		var dy = position.Y - dragStart.Y;
		CenterLogicalPos += new Avalonia.Point(dx, dy);
	}

	protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
	{
		const float zoomChangeStep = 1.1f;
		switch (e.Delta.Y)
		{
			case > 0:
				ZoomScale *= zoomChangeStep;
				break;
			case < 0:
				ZoomScale /= zoomChangeStep;
				break;
		}
	}

	public override void Render(DrawingContext drawingContext)
	{
		if (painter == null) return;

		UpdateScale();


		painter.Paint(drawingContext, ZoomScale);

		if (mouseLogicalPos.X == 0 || mouseLogicalPos.Y == 0) return;
		
		var typeface = new Typeface("Segoe UI Light");
		const int fontSize = 16;
		var formattedText = new FormattedText(
			$"(X: {Math.Floor(mouseLogicalPos.X / painter.CellSize.Width)}; Y: {Math.Floor(mouseLogicalPos.Y / painter.CellSize.Height)})",
			typeface,
			fontSize / ZoomScale,
			TextAlignment.Left,
			TextWrapping.NoWrap,
			Size.Infinity);
		drawingContext.DrawText(Brushes.Red, mouseLogicalPos, formattedText);
	}
}