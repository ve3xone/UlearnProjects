namespace linq_slideviews;

public class SlideRecord
{
	public SlideRecord(int slideId, SlideType slideType, string unitTitle)
	{
		SlideId = slideId;
		SlideType = slideType;
		UnitTitle = unitTitle;
	}

	public readonly int SlideId;
	public readonly SlideType SlideType;
	public readonly string UnitTitle;

	public override string ToString()
	{
		return $"{nameof(SlideId)}: {SlideId}, {nameof(SlideType)}: {SlideType}, {nameof(UnitTitle)}: {UnitTitle}";
	}

	protected bool Equals(SlideRecord other)
	{
		return SlideId == other.SlideId && SlideType == other.SlideType && string.Equals(UnitTitle, other.UnitTitle);
	}

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((SlideRecord) obj);
	}

	public override int GetHashCode()
	{
		unchecked
		{
			var hashCode = SlideId;
			hashCode = (hashCode*397) ^ (int) SlideType;
			hashCode = (hashCode*397) ^ (UnitTitle != null ? UnitTitle.GetHashCode() : 0);
			return hashCode;
		}
	}
}