using System.Linq;
using NUnit.Framework;

namespace yield;

[TestFixture]
public class ExpSmoothingTests
{
	[Test]
	public void SmoothEmptySequence()
	{
		CheckSmooth(0.1, new double[] { }, new double[] { });
	}

	[Test]
	public void SmoothSingleZero()
	{
		CheckSmooth(0.1, new[] { 0.0 }, new[] { 0.0 });
	}

	[Test]
	public void SmoothSingleNonZeroValue()
	{
		CheckSmooth(0.5, new[] { 100.0 }, new[] { 100.0 });
	}

	[Test]
	public void SmoothTwoValues()
	{
		CheckSmooth(0.1, new[] { 0, 20.0 }, new[] { 0, 2.0 });
	}

	[Test]
	public void SmoothTwoValues2()
	{
		CheckSmooth(0.1, new[] { 10, 0.0 }, new[] { 10, 9.0 });
	}

	[Test]
	public void SmoothWithZeroAlpha()
	{
		CheckSmooth(0.0, new double[] { 1, 2, 3, 4, 5, 6 }, new double[] { 1, 1, 1, 1, 1, 1 });
	}

	[Test]
	public void SmoothWithOneAlpha()
	{
		CheckSmooth(1.0, new double[] { 1, 2, 3, 4, 5, 6 }, new double[] { 1, 2, 3, 4, 5, 6 });
	}

	private void CheckSmooth(double alpha, double[] ys, double[] expectedYs)
	{
		var dataPoints = ys.Select((v, index) => new DataPoint(GetX(index), v));
		var actual = Factory.CreateAnalyzer().SmoothExponentialy(dataPoints, alpha).ToList();
		Assert.AreEqual(ys.Length, actual.Count);
		for (var i = 0; i < actual.Count; i++)
		{
			Assert.AreEqual(GetX(i), actual[i].X, 1e-7);
			Assert.AreEqual(ys[i], actual[i].OriginalY, 1e-7);
			Assert.AreEqual(expectedYs[i], actual[i].ExpSmoothedY, 1e-7);
		}
	}

	private double GetX(int index)
	{
		return (index - 3.0)/2;
	}
}