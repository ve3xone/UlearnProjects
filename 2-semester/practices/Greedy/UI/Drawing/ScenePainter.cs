using System.Linq;
using Avalonia;
using Avalonia.Media;
using Greedy.Architecture;
using Brushes = Avalonia.Media.Brushes;
using Pen = Avalonia.Media.Pen;
using Point = Greedy.Architecture.Point;
using Size = Avalonia.Size;

namespace Greedy.UI.Drawing;

public class ScenePainter : IScenePainter
{
	public Size RealSize => Controller == null
		? new Size(1, 1)
		: new Size(state.MapWidth * CellSize.Width, state.MapHeight * CellSize.Height + 10);

	public GreedyCanvas GreedyCanvas { get; }

	public Size CellSize => Sprites.Wall.Size;
	private State state => Controller.State;


	public StateController Controller { get; set; }

	public ScenePainter(GreedyCanvas greedyCanvas)
	{
		GreedyCanvas = greedyCanvas;
		greedyCanvas.painter = this;
	}

	public void Paint(DrawingContext drawingContext, double zoomScale)
	{
		if (Controller == null)
			return;

		DrawMap(drawingContext);
		DrawDigitsAndChests(drawingContext, zoomScale);
		DrawMovements(drawingContext);
		DrawPlayer(drawingContext);
	}

	private void DrawDigitsAndChests(DrawingContext drawingContext, double zoomScale)
	{
		var typeface = new Typeface("Segoe UI Light");
		var fontSize = 25;

		var height = state.MapHeight;
		var width = state.MapWidth;

		for (var y = 0; y < height; y++)
		for (var x = 0; x < width; x++)
		{
			var logicalLocation = new Point(x, y);
			var realPosition =  new Avalonia.Point(x * CellSize.Width, y * CellSize.Height);
			
			if (state.IsWallAt(logicalLocation)) continue;
			
			var drawDigits = zoomScale > 0.5;
			if (state.IsChestAt(logicalLocation))
			{
				var chestLocation = drawDigits
					? new Avalonia.Point(realPosition.X + 30, realPosition.Y + 30)
					: new Avalonia.Point(realPosition.X + 15, realPosition.Y + 15);

				var chestRect = new Rect(chestLocation,
					new Size(CellSize.Height - 25, CellSize.Width - 30)
				);
				var isOpened = Controller.VisitedChests.Contains(logicalLocation);

				drawingContext.DrawImage(isOpened
						? Sprites.ChestOpened
						: Sprites.ChestClosed,
					chestRect);
			}

			if (!drawDigits) continue;

			var formattedText = new FormattedText(
				state.CellCost[x, y].ToString(),
				typeface,
				fontSize,
				TextAlignment.Left,
				TextWrapping.NoWrap,
				Size.Infinity);
			drawingContext.DrawText(Brushes.Black, realPosition, formattedText);
		}
	}

	private void DrawMovements(DrawingContext drawingContext)
	{
		var pen = new Pen(Brushes.DarkBlue, 2f);
		foreach (var logicalTo in Controller.MovementsToFrom.Keys)
		{
			var logicalsFrom = Controller.MovementsToFrom[logicalTo];
			var physicalTo = logicalTo.MultiplyTransform(CellSize.Height);
			var currentCenter = new Rect(physicalTo, CellSize).Center;

			foreach (var prevCenter in logicalsFrom
				         .Select(logicalFrom => logicalFrom.MultiplyTransform(CellSize.Height))
				         .Select(physicalFrom => new Rect(physicalFrom, CellSize).Center))
				drawingContext.DrawLine(pen, prevCenter, currentCenter);
		}
	}

	private void DrawPlayer(DrawingContext drawingContext)
	{
		if (!state.InsideMap(state.Position)) return;

		var playerLocation = state.Position.MultiplyTransform(CellSize.Height);

		if (Controller.GameIsLost)
			playerLocation += new Avalonia.Point(0, 20);

		drawingContext.DrawImage(
			PlayerImageProvider.ProvidePlayerImage(Controller),
			new Rect(playerLocation, CellSize)
		);
	}

	private void DrawMap(DrawingContext drawingContext)
	{
		var cost = GetMinMax(state.CellCost);

		var height = state.MapHeight;
		var width = state.MapWidth;
		
		for (var y = 0; y < height; y++)
		for (var x = 0; x < width; x++)
		{
			var logicalPosition = new Point(x, y);
			var realPosition = new Avalonia.Point(x * CellSize.Width, y * CellSize.Height);
			var cellBounds = new Rect(realPosition, CellSize);

			if (state.IsWallAt(logicalPosition))
				drawingContext.DrawImage(Sprites.Wall, cellBounds);
			else
			{
				var gradientBrushForCell = GetGradientBrushForCell(state.CellCost[x, y], cost);
				drawingContext.FillRectangle(gradientBrushForCell.Brush, cellBounds);
			}
		}
	}

	private Pen GetGradientBrushForCell(int density, Point minMax)
	{
		var levelsCount = minMax.Y - minMax.X;
		var delta = 50 / levelsCount;
		var color = Color.FromArgb((byte)(density * delta), Colors.Red.R, Colors.Red.G, Colors.Red.B);
		return new Pen(color.ToUint32());
	}

	private Point GetMinMax(int[,] map)
	{
		var kx = 0;
		var ky = 0;
		var height = map.GetLength(1);
		var width = map.GetLength(0);

		for (var y = 0; y < height; y++)
		for (var x = 0; x < width; x++)
			if (map[x, y] > ky)
				ky = map[x, y];
			else if (map[x, y] < kx)
				kx = map[x, y];

		return new Point(kx, ky);
	}
}