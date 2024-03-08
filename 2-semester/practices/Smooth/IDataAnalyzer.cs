using System.Collections.Generic;

namespace yield;

public interface IDataAnalyzer
{
	IEnumerable<DataPoint> SmoothExponentialy(IEnumerable<DataPoint> data, double alpha);
	IEnumerable<DataPoint> MovingAverage(IEnumerable<DataPoint> data, int windowWidth);
	IEnumerable<DataPoint> MovingMax(IEnumerable<DataPoint> data, int windowWidth);
}

public class DataAnalyzerImpl : IDataAnalyzer
{
	public IEnumerable<DataPoint> SmoothExponentialy(IEnumerable<DataPoint> data, double alpha)
	{
		return data.SmoothExponentialy(alpha);
	}

	public IEnumerable<DataPoint> MovingAverage(IEnumerable<DataPoint> data, int windowWidth)
	{
		return data.MovingAverage(windowWidth);
	}

	public IEnumerable<DataPoint> MovingMax(IEnumerable<DataPoint> data, int windowWidth)
	{
		return data.MovingMax(windowWidth);
	}
}