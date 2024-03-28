using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace linq_slideviews;

[TestFixture]
public class ParsingTests
{
	private string slidesHeaderLine = "SlideId;SlideType;UnitTitle";
	private string visitsHeaderLine = "UserId;SlideId;Date;Time";

	[Test]
	public void ParseEmptySlides()
	{
		var dict = ParsingTask.ParseSlideRecords(new[] { slidesHeaderLine });
		Assert.That(dict, Is.Empty);
	}

	[Test]
	public void ParseEmptyVisits()
	{
		var visits = ParsingTask.ParseVisitRecords(new[] { visitsHeaderLine }, new Dictionary<int, SlideRecord>());
		Assert.That(visits, Is.Empty);
	}

	[Test]
	public void SkipIncorrectSlides()
	{
		var dict = ParsingTask.ParseSlideRecords(new[] { slidesHeaderLine, "asd;asd;asd" });
		Assert.That(dict, Is.Empty);
	}

	[Test]
	public void ParseSeveralSlides()
	{
		var dict =
			ParsingTask.ParseSlideRecords(new[] { slidesHeaderLine, "42;exercise;title", "1;quiz;title2" });
		Assert.That(dict, Has.Count.EqualTo(2));
		Assert.That(dict[42], Is.EqualTo(new SlideRecord(42, SlideType.Exercise, "title")));
		Assert.That(dict[1], Is.EqualTo(new SlideRecord(1, SlideType.Quiz, "title2")));
	}

	[Test]
	public void ParseSingleSlide()
	{
		var dict = ParsingTask.ParseSlideRecords(new[] { slidesHeaderLine, "42;exercise;title" });
		Assert.That(dict, Has.Count.EqualTo(1));
		Assert.That(dict[42], Is.EqualTo(new SlideRecord(42, SlideType.Exercise, "title")));
	}

	[Test]
	public void ParseVisits_Fails_OnIncorrectLine()
	{
		var lines = new[] { visitsHeaderLine, "very wrong line!" };
		var exception = Assert.Throws<FormatException>(() =>
			ParsingTask.ParseVisitRecords(lines, new Dictionary<int, SlideRecord>()).ToList()
		);

		Assert.That(exception.Message, Is.EqualTo("Wrong line [very wrong line!]"));

	}

	[Test]
	public void ParseVisits_Fails_OnIncorrectDate()
	{
		var lines = new[] { visitsHeaderLine, "1;2;2000-13-30;12:00:00" };
		var exception = Assert.Throws<FormatException>(() =>
			ParsingTask.ParseVisitRecords(lines, new Dictionary<int, SlideRecord>()).ToList()
		);

		Assert.That(exception.Message, Is.EqualTo("Wrong line [1;2;2000-13-30;12:00:00]"));
	}

	[Test]
	public void ParseVisits_Fails_OnIncorrectTime()
	{
		var lines = new[] { visitsHeaderLine, "1;2;2000-01-30;27:99:00" };
		var exception = Assert.Throws<FormatException>(() =>
			ParsingTask.ParseVisitRecords(lines, new Dictionary<int, SlideRecord>()).ToList()
		);

		Assert.That(exception.Message, Is.EqualTo("Wrong line [1;2;2000-01-30;27:99:00]"));
	}

	[Test]
	public void ParseVisits()
	{
		var visits = ParsingTask.ParseVisitRecords(
			new[]
			{
				visitsHeaderLine,
				"1;2;2000-01-30;12:00:00",
				"2;2;2000-01-31;12:01:00",
				"1;3;2000-01-29;01:00:00",
				"3;4;2000-02-01;09:10:11"
			},
			new Dictionary<int, SlideRecord>
			{
				{ 2, new SlideRecord(2, SlideType.Quiz, "quiz slide") },
				{ 3, new SlideRecord(3, SlideType.Exercise, "exercise slide") },
				{ 4, new SlideRecord(4, SlideType.Theory, "theory slide") },
				{ 5, new SlideRecord(5, SlideType.Theory, "theory slide") }
			});
		Assert.That(visits, Is.EqualTo(new[]
		{
			new VisitRecord(1, 2, new DateTime(2000, 01, 30, 12, 0, 0), SlideType.Quiz),
			new VisitRecord(2, 2, new DateTime(2000, 01, 31, 12, 1, 0), SlideType.Quiz),
			new VisitRecord(1, 3, new DateTime(2000, 01, 29, 01, 0, 0), SlideType.Exercise),
			new VisitRecord(3, 4, new DateTime(2000, 02, 01, 09, 10, 11), SlideType.Theory)
		}));
	}
}