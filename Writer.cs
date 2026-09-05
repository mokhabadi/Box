using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Box;

public class Writer(BinaryWriter binaryWriter) : IWriter
{
	public void Write<T>(T value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		WriteTypeAndKey<T>(key);
		WriteValue(value);
		WriteChar('|');
	}

	public void WriteNullable<T>(T? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		WriteHasValue(value);
		WriteTypeAndKey<T>(key);
		if (value != null) WriteValue(value);
		WriteChar('|');
	}

	public bool Write<T>(T[] value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		WriteChar('A');
		binaryWriter.Write7BitEncodedInt(value.Length);
		WriteTypeAndKey<T>(key);
		WriteChar('[');
		foreach (T item in value) WriteValue(item);
		WriteChar(']');
		WriteChar('|');
		return true;
	}

	public bool WriteNullable<T>(T[]? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		WriteHasValue(value);
		if (value != null) return Write(value, key);
		WriteChar('A');
		WriteTypeAndKey<T>(key);
		WriteChar('|');
		return true;
	}

	private void WriteTypeAndKey<T>(string key)
	{
		Type type = typeof(T);
		type = Nullable.GetUnderlyingType(type) ?? type;
		if (type.IsAssignableTo(typeof(IBox))) type = typeof(object);
		string typeName = type.Name;
		binaryWriter.Write(typeName);
		binaryWriter.Write(key);
	}

	private void WriteHasValue<T>(T? value)
	{
		char hasValue = value != null ? 'T' : 'F';
		WriteChar(hasValue);
	}

	private void WriteChar(char value)
	{
		binaryWriter.Write(value);
	}

	private void WriteValue<T>(T value)
	{
		Action action = value switch
		{
			bool @bool => () => binaryWriter.Write(@bool),
			char @char => () => binaryWriter.Write(@char),
			byte @byte => () => binaryWriter.Write(@byte),
			sbyte @sbyte => () => binaryWriter.Write(@sbyte),
			short @short => () => binaryWriter.Write(@short),
			ushort @ushort => () => binaryWriter.Write(@ushort),
			int @int => () => binaryWriter.Write(@int),
			uint @uint => () => binaryWriter.Write(@uint),
			long @long => () => binaryWriter.Write(@long),
			ulong @ulong => () => binaryWriter.Write(@ulong),
			float @float => () => binaryWriter.Write(@float),
			double @double => () => binaryWriter.Write(@double),
			decimal @decimal => () => binaryWriter.Write(@decimal),
			string @string => () => binaryWriter.Write(@string),
			DateTime dateTime => () => binaryWriter.Write(dateTime.ToBinary()),
			TimeSpan timeSpan => () => binaryWriter.Write(timeSpan.Ticks),
			object @object => () => WriteObject(@object),
			_ => throw new NotSupportedException(typeof(T).Name)
		};

		action();
	}

	private void WriteObject(object @object)
	{
		WriteChar('{');
		((IBox)@object).WriteTo(this);
		WriteChar('}');
	}

	public static byte[] WriteToByteArray(IBox box)
	{
		using MemoryStream memoryStream = new();
		using BinaryWriter binaryWriter = new(memoryStream);
		Writer writer = new(binaryWriter);
		writer.Write(box);
		byte[] bytes = memoryStream.ToArray();
		return bytes;
	}
}