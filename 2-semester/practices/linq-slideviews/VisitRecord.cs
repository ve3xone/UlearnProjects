using System;

namespace linq_slideviews;

public record VisitRecord(int UserId, int SlideId, DateTime DateTime, SlideType SlideType)
{
	public override string ToString() =>
		$"{nameof(UserId)}: {UserId}, {nameof(SlideId)}: {SlideId}, {nameof(SlideType)}: {SlideType}, {nameof(DateTime)}: {DateTime}";

	public override int GetHashCode()
	{
		unchecked
		{
			var hashCode = UserId;
			hashCode = (hashCode * 397) ^ SlideId;
			hashCode = (hashCode * 397) ^ (int)SlideType;
			hashCode = (hashCode * 397) ^ DateTime.GetHashCode();
			return hashCode;
		}
	}
}