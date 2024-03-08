using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;

namespace yield.UI;

public class MainViewModel : IGraphModel
{
	private readonly LineSeries originalPoints;
	private readonly LineSeries expPoints;
	private readonly LineSeries avgPoints;
	private readonly LineSeries maxPoints;

	public PlotModel Model { get; }
	public IPlotController Controller { get; }

	public MainViewModel()
	{
		Controller = new PlotController();
		Model = new PlotModel { Title = "Сравнение методов сглаживания" };

		Model.Legends.Add(new Legend
		{
			LegendPosition = LegendPosition.RightTop
		});

		Model.Axes.Add(new LinearAxis
		{
			Title = "X",
			Position = AxisPosition.Bottom,
			MinimumPadding = 0,
			Minimum = 0,
			IntervalLength = 100
		});

		var line = new LineAnnotation
		{
			LineStyle = LineStyle.Solid,
			Color = OxyColors.Black,
			Type = LineAnnotationType.Horizontal,
			TextColor = OxyColors.Black,
		};

		Model.Annotations.Add(line);
		Model.Axes.Add(new LinearAxis
		{
			Title = "Y",
			Position = AxisPosition.Left,
			MinimumPadding = 0,
			Minimum = -5,
			Maximum = 35,
			IntervalLength = 25
		});

		originalPoints = AddCurve("original", OxyColors.Black);
		avgPoints = AddCurve("avg", OxyColors.Blue);
		expPoints = AddCurve("exp", OxyColors.Red);
		maxPoints = AddCurve("max", OxyColors.Green);


		Model.Series.Add(originalPoints);
		Model.Series.Add(avgPoints);
		Model.Series.Add(expPoints);
		Model.Series.Add(maxPoints);
	}

	private LineSeries AddCurve(string label, OxyColor color)
	{
		return new LineSeries
		{
			Color = color,
			Title = label,
		};
	}

	public void AddPoint(DataPoint p)
	{
		originalPoints.Points.Add(new(p.X, p.OriginalY));
		avgPoints.Points.Add(new(p.X, p.AvgSmoothedY));
		expPoints.Points.Add(new(p.X, p.ExpSmoothedY));
		maxPoints.Points.Add(new(p.X, p.MaxY));
	}
}