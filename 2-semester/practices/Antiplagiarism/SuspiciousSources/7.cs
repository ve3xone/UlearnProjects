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
		{
			throw new Exception($"Wrong test map '{map}'");
		}

		var result = new ICreature[rows[0].Length, rows.Length];
		for (var x = 0; x < rows[0].Length; x++)
		{
			for (var y = 0; y < rows.Length; y++)
			{
				result[x, y] = CreateCreatureBySymbol(rows[y][x]);
			}
		}

		return result;
	}

	private static ICreature CreateCreatureByTypeName(string name)
	{
		if (factory.ContainsKey(name)) 
			return factory[name]();
		var type = Assembly
			.GetExecutingAssembly()
			.GetTypes()
			.FirstOrDefault(z => z.Name == name);
		if (type == null)
		{
			throw new Exception($"Can't find type '{name}'");
		}

		factory[name] = () => (ICreature) Activator.CreateInstance(type);

		return factory[name]();
	}


	private static ICreature CreateCreatureBySymbol(char c)
	{
		if (c == 'P')
		{
			return CreateCreatureByTypeName("Player");
		}
		if (c == 'T')
		{
			return CreateCreatureByTypeName("Terrain");
		}
		if (c == 'G')
		{
			return CreateCreatureByTypeName("Gold");
		}
		if (c == 'S')
		{
			return CreateCreatureByTypeName("Sack");
		}
		if (c == 'M')
		{
			return CreateCreatureByTypeName("Monster");
		}
		if (c == ' ')
		{
			return null;
		}

		throw new Exception($"wrong character for ICreature {c}");
	}
}