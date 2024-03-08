using System;
using System.Collections.Generic;

namespace yield;

public class DataSource
{
	public static IEnumerable<DataPoint> GetData(Random random)
	{
		//var dataAnalyzer = Factory.CreateAnalyzer();
		//return dataAnalyzer.MovingMax(dataAnalyzer.MovingAverage(dataAnalyzer.SmoothExponentialy(GenerateOriginalData(random), 0.2), 20),150);
		return GenerateOriginalData(random).SmoothExponentialy(0.2).MovingAverage(20).MovingMax(150);
	}

	public static IEnumerable<DataPoint> GenerateOriginalData(Random random)
	{
		var x = 0;
		while (true)
		{
			x++;
			var y = 10 * (1 - x / 100 % 2) + 3 * Math.Sin(x / 40.0) + 2 * random.NextDouble() - 1 + 3;
			yield return new DataPoint(x, y);
		}
	}

}