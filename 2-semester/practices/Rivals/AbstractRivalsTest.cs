using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Rivals;

public class AbstractRivalsTest
{
	protected static void AssertResult(IEnumerable<OwnedLocation> expectedResult, IEnumerable<OwnedLocation> actualResult)
	{
		var actual = actualResult.ToLookup(l => l.Location);
		var expected = expectedResult.ToDictionary(l => l.Location);
		foreach (var expectedLocation in expected)
		{
			var actualLocation = actual[expectedLocation.Key].ToArray();
			if (actualLocation.Length > 1)
			{
				var allActual = string.Join<OwnedLocation>(", ", actualLocation);
				Assert.Fail($"Multiple OwnedLocation's with same location {expectedLocation.Key}: {allActual}");
			}
			if (actualLocation.Length == 0)
			{
				Assert.Fail($"Not found OwnedLocation with location: {expectedLocation.Key}");
			}
			Assert.AreEqual(expectedLocation.Value, actualLocation[0]);
		}
		foreach (var location in actual)
		{
			if (!expected.ContainsKey(location.Key))
				Assert.Fail($"Found extra OwnedLocation: {location.First()}");
		}
	}

}