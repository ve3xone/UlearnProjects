using System;
using System.Collections.Generic;

namespace Antiplagiarism;

public class ComparisonResult
{
	public readonly double Distance;
	public readonly List<string> Document1;
	public readonly List<string> Document2;

	public ComparisonResult(List<string> document1, List<string> document2, double distance)
	{
		Document1 = document1;
		Document2 = document2;
		Distance = distance;
	}

	public override bool Equals(object obj)
	{
		if (obj == this) return true;

		if (obj is not ComparisonResult item) return false;

		return Equals(item);
	}

	protected bool Equals(ComparisonResult other)
	{
		return (Equals(Document1, other.Document1) && Equals(Document2, other.Document2) ||
		        Equals(Document1, other.Document2) && Equals(Document2, other.Document1))
		       && AreEquals(Distance, other.Distance);
	}

	public static bool AreEquals(double first, double second)
	{
		const double epsilon = 1e-6;
		return Math.Abs(first - second) <= epsilon;
	}

	public override string ToString()
	{
		return $"{nameof(Distance)}: {Distance}";
	}

	public override int GetHashCode()
	{
		var hashCode = Document1 != null ? Document1.GetHashCode() : 0;
		hashCode ^= Document2 != null ? Document2.GetHashCode() : 0;
		return hashCode;
	}
}