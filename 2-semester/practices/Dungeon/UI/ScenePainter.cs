using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Dungeon.UI;

public class ScenePainter : Canvas
{
	private Dictionary<Map, Point[]> paths;
	private Map currentMap;

	private int mainIteration;

	private Point lastMouseClick;
	private IEnumerable<List<Point>> pathsToChests;
	private Bitmap grass;
	private Bitmap path;
	private Bitmap peasant;
	private Bitmap castle;
	private Bitmap chest;

	private double cellWidth => grass.Size.Width;
	private double cellHeight => grass.Size.Height;

	public ScenePainter()
	{
		LoadResources();
	}

	public void Load(Map[] maps)
	{
		paths = maps
			.ToDictionary(x => x, x => TransformPath(x, DungeonTask.FindShortestPath(x))
				.ToArray());

		currentMap = maps[0];
		mainIteration = 0;
	}

	// Loading non-string resources via ResourcesManages causes error with BuildTools2022
	private void LoadResources()
	{
		var resourcesPrefix = "UI/Images/";

		grass = new Bitmap($"{resourcesPrefix}Grass.png");
		path = new Bitmap($"{resourcesPrefix}Path.png");
		peasant = new Bitmap($"{resourcesPrefix}Peasant.png");
		castle = new Bitmap($"{resourcesPrefix}Castle.png");
		chest = new Bitmap($"{resourcesPrefix}Chest.png");
	}

	public void ChangeLevel(Map newMap)
	{
		currentMap = newMap;
		mainIteration = 0;
		lastMouseClick = null;
		pathsToChests = null;
		InvalidateVisual();
	}

	public void Update()
	{
		mainIteration = Math.Min(mainIteration + 1, paths[currentMap].Length - 1);
		InvalidateVisual();
	}

	protected override void OnPointerPressed(PointerPressedEventArgs e)
	{
		var location = e.GetPosition(this);
		var position = new Point((int)(location.X / cellWidth), (int)(location.Y / cellHeight));

		lastMouseClick = position;
		pathsToChests = null;
		if (!currentMap.InBounds(position) ||
		    currentMap.Dungeon[lastMouseClick.X, lastMouseClick.Y] != MapCell.Empty) return;
		
		pathsToChests = BfsTask.FindPaths(currentMap, lastMouseClick, currentMap.Chests)
			.Select(x => x.ToList()).ToList();
		
		foreach (var pathsToChest in pathsToChests)
			pathsToChest.Reverse();
	}
	
	protected override void OnPointerReleased(PointerReleasedEventArgs e)
	{
		pathsToChests = null;
	}

	public override void Render(DrawingContext context)
	{
		base.Render(context);
		DrawLevel(context);
		DrawMainPath(context, mainIteration);
		if (pathsToChests != null && lastMouseClick.HasValue)
			DrawAdditionalPaths(context, lastMouseClick);
	}

	private void DrawLevel(DrawingContext context)
	{
		RenderMap(context);
		foreach (var chestPoint in currentMap.Chests)
			context.DrawImage(chest,
				new Rect(chestPoint.X * cellWidth, chestPoint.Y * cellHeight, cellWidth, cellHeight));
		context.DrawImage(castle,
			new Rect(currentMap.Exit.X * cellWidth, currentMap.Exit.Y * cellHeight, cellWidth, cellHeight));
	}

	private void DrawPath(DrawingContext context, Color color, IEnumerable<Point> path)
	{
		var points = path.Select(x =>
			new Avalonia.Point(x.X * cellWidth + cellWidth * 0.5f, x.Y * cellHeight + cellHeight * 0.5f)).ToArray();
		var newStyle = new DashStyle(new[] { cellWidth * 0.125f, cellHeight * 0.125f }, 1d);
		var pen = new Pen(color.ToUint32(), cellHeight * 0.125f, newStyle);
		for (var i = 0; i < points.Length - 1; i++)
			context.DrawLine(pen, points[i], points[i + 1]);
	}

	private void DrawMainPath(DrawingContext context, int interation)
	{
		var path = paths[currentMap].Take(interation + 1).ToArray();
		DrawPath(context, Colors.Green, path);
		var position = path[^1];
		context.DrawImage(peasant, new Rect(position.X * cellWidth, position.Y * cellHeight, cellWidth, cellHeight));
	}

	private void DrawAdditionalPaths(DrawingContext context, Point lastClick)
	{
		context.FillRectangle(Brushes.Red,
			new Rect(lastClick.X * cellWidth, lastClick.Y * cellHeight, cellWidth, cellHeight));
		foreach (var pathToChest in pathsToChests)
			DrawPath(context, Colors.Red, pathToChest);
	}

	private IEnumerable<Point> TransformPath(Map map, MoveDirection[] path)
	{
		var walker = new Walker(map.InitialPosition);
		yield return map.InitialPosition;
		foreach (var direction in path)
		{
			walker = walker.WalkInDirection(map, direction);
			yield return walker.Position;
			if (walker.PointOfCollision.HasValue)
				break;
		}
	}

	private void RenderMap(DrawingContext context)
	{
		var cellWidth = grass.Size.Width;
		var cellHeight = grass.Size.Height;
		var width = currentMap.Dungeon.GetLength(0);
		var height = currentMap.Dungeon.GetLength(1);
		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				var image = currentMap.Dungeon[x, y] == MapCell.Wall ? grass : path;
				context.DrawImage(image, new Rect(x * cellWidth, y * cellHeight, cellWidth, cellHeight));
			}
		}
	}
}