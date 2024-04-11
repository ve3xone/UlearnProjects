using System.ComponentModel;
using System.IO;

namespace Dungeon.Dungeons;

public static class DungeonsLoader
{
	public static string Load(DungeonsName dungeonName)
	{
		return File.ReadAllText("Dungeons/" + dungeonName.ToDescriptionString());
	}
}

public enum DungeonsName
{
	[Description("BigTestDungeon.txt")] BigDungeon,
	[Description("Dungeon1.txt")] Dungeon1,
	[Description("Dungeon2.txt")] Dungeon2,
	[Description("Dungeon3.txt")] Dungeon3,
	[Description("Dungeon4.txt")] Dungeon4,
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