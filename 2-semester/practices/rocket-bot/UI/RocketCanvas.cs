using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace rocket_bot.UI;

public class RocketCanvas : Canvas
{
	public Image RocketImage;
	public RocketModel model;
	private const string prefix = "UI/images";
	private readonly Bitmap rocketBitmap = new(prefix + "/rocket.png");
	private readonly Bitmap flagBitmap = new(prefix + "/flag.png");
	
	
	public override void Render(DrawingContext context)
	{ 
		var (level, rocket, channel) = model.Data; 
		context.FillRectangle(Brushes.Beige, Bounds);

		if (level == null || rocket == null) return;

		var pen = new Pen();
		for (var i = 0; i < level.Checkpoints.Length; ++i)
		{
			var flagSize = flagBitmap.Size;
			var flagCenter = new Point(
				level.Checkpoints[i].X,
				level.Checkpoints[i].Y
			);
			var flagPosition = new Point(
				flagCenter.X - flagBitmap.Size.Width / 2,
				flagCenter.Y - flagBitmap.Size.Height / 2
			);
			var flagRect = new Rect(flagPosition, flagSize);
			context.DrawImage(flagBitmap, flagRect);

			if (rocket.TakenCheckpointsCount % level.Checkpoints.Length == i)
				context.DrawEllipse(Brushes.Gold, pen, flagCenter, 10, 10);
		}

		RocketImage.Margin = new Thickness(rocket.Location.X - rocketBitmap.Size.Width / 2,
			rocket.Location.Y - rocketBitmap.Size.Height / 2);
		RocketImage.Source = rocketBitmap;
		RocketImage.RenderTransform = new RotateTransform(90 + (float)(rocket.Direction * 180 / Math.PI));
	}
}