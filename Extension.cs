using System;
using System.Collections.Generic;

namespace Box;

public static class Extension
{
	private static readonly Dictionary<Type, char> typeIdDictionary = new()
	{
		{ typeof(bool), 'b' },
		{ typeof(byte), 'B' },
		{ typeof(char), 'c' },
		{ typeof(int), 'i' },
		{ typeof(long), 'L' },
		{ typeof(long?), 'L' },
		{ typeof(double), 'd' },
		{ typeof(decimal), 'm' },
		{ typeof(string), 's' },
		{ typeof(DateTime), 'D' },
		{ typeof(TimeSpan), 'T' },
		{ typeof(IBox), 'O' },
	};

	public static char GetTypeId(this Type type)
	{
		return typeIdDictionary[type];
	}
}