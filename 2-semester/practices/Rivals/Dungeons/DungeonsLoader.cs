using System.ComponentModel;
using System.IO;

namespace Rivals.Dungeons;

public static class DungeonsLoader
{
	public static string Load(DungeonsName dungeonName)
	{
		return File.ReadAllText("Dungeons/" + dungeonName.ToDescriptionString());
	}
}

public enum DungeonsName
{
	[Description("Dungeon1.txt")] Dungeon1,
	[Description("Dungeon2.txt")] Dungeon2,
	[Description("Dungeon3.txt")] Dungeon3,
	[Description("Dungeon4.txt")] Dungeon4,
	[Description("Dungeon5.txt")] Dungeon5,
}

public static class DungeonsNameExtensions
{
	public static string ToDescriptionString(this DungeonsName val)
	{
		var attributes = (DescriptionAttribute[])val
			.GetType()
			.GetField(val.ToString())
			.GetCustomAttributes(typeof(DescriptionAttribute), false);
		return attributes.Length > 0 ? attributes[0].Description : string.Empty;
	}
}