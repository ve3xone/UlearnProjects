namespace yield;

public class DataPoint
{
	public readonly double X;
	public readonly double OriginalY;
	public double ExpSmoothedY { get; private set; }
	public double AvgSmoothedY { get; private set; }
	public double MaxY { get; private set; }

	public DataPoint(double x, double y)
	{
		X = x;
		OriginalY = y;
	}

	public DataPoint(DataPoint point)
	{
		X = point.X;
		OriginalY = point.OriginalY;
		ExpSmoothedY = point.ExpSmoothedY;
		AvgSmoothedY = point.AvgSmoothedY;
		MaxY = point.MaxY;
	}

	public DataPoint WithExpSmoothedY(double expSmoothedY)
	{
		return new DataPoint(this) { ExpSmoothedY = expSmoothedY };
	}

	public DataPoint WithAvgSmoothedY(double avgSmoothedY)
	{
		return new DataPoint(this) { AvgSmoothedY = avgSmoothedY };
	}

	public DataPoint WithMaxY(double maxY)
	{
		return new DataPoint(this) { MaxY = maxY };
	}
}