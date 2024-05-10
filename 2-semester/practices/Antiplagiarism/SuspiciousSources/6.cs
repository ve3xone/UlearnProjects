using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Digger;

public static class CreatureMapCreator
{
	private static readonly Dictionary<string, Func<ICreature>> factory = new();

	public static ICreature[,] CreateMap(string map, string separator = "\r\n")
	{
		var rows = map.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries);
		if (rows.Select(z => z.Length).Distinct().Count() != 1)
			throw new Exception($"Wrong test map '{map}'");
		var result = new ICreature[rows[0].Length, rows.Length];
		for (var x = 0; x < rows[0].Length; x++)
		for (var y = 0; y < rows.Length; y++)
			result[x, y] = CreateCreatureBySymbol(rows[y][x]);
		return result;
	}

	private static ICreature CreateCreatureByTypeName(string name)
	{
		// Какой-то комментарий
		// И ещё..
		// И ещё комментарий
		// Пожалуй, перепишу один метод
		if (!factory.ContainsKey(name))
		{
			var type = Assembly
				.GetExecutingAssembly()
				.GetTypes()
				.FirstOrDefault(z => z.Name == name);
			if (type == null)
				throw new Exception($"Can't find type '{name}'");
			factory[name] = () => (ICreature) Activator.CreateInstance(type);
		}

		return factory[name]();
	}


	private static ICreature CreateCreatureBySymbol(char c)
	{
		List<string> names = new List<string> {"Player", "Terrain", "Gold", "Sack", "Monster"};
		if (c == ' ')
		{
			return null;
		}

		var name = names.FirstOrDefault(s => s.StartsWith(new string(c, 1)));
		if (name == null)
		{
			throw new Exception($"wrong character for ICreature {c}");
		}

		return CreateCreatureByTypeName(name);
	}
}