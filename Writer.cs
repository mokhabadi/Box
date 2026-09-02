using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Box;

public class Writer(BinaryWriter binaryWriter) : IWriter
{
	public void Write<T>(T value, [CallerArgumentExpression(nameof(value))] string? key = null)
	{
		WriteKey(key);
		WriteType<T>();
		WriteValue(value);
	}

	public void Write<T>(T[] value, [CallerArgumentExpression(nameof(value))] string? key = null)
	{
		WriteKey(key);
		WriteChar('A');
		WriteLength(value.Length);
		WriteType<T>();
		WriteChar('[');
		foreach (T item in value) WriteValue(item);
		WriteChar(']');
	}

	public void WriteNullable<T>(T? value, [CallerArgumentExpression(nameof(value))] string? key = null)
	{
		WriteKey(key);
		WriteHasValue(value);
		WriteType<T>();
		if (value != null) WriteValue(value);
	}

	public void WriteNullable<T>(T[]? value, [CallerArgumentExpression(nameof(value))] string? key = null)
	{
		WriteKey(key);
		WriteHasValue(value);
		WriteChar('A');
		if (value != null) WriteLength(value.Length);
		WriteType<T>();
		if (value == null) return;
		WriteChar('[');
		foreach (T item in value) WriteValue(item);
		WriteChar(']');
	}

	private void WriteKey(string? key)
	{
		if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key is null or empty", nameof(key));
		WriteChar('|');
		binaryWriter.Write(key);
	}

	private void WriteType<T>()
	{
		Type type = typeof(T);
		Type? nullableType = Nullable.GetUnderlyingType(type);
		string typeName = nullableType != null ? nullableType.Name : type.Name;
		binaryWriter.Write(typeName);
	}

	private void WriteChar(char value)
	{
		binaryWriter.Write(value);
	}

	private void WriteHasValue<T>(T? value)
	{
		char hasValue = value != null ? 'T' : 'F';
		binaryWriter.Write(hasValue);
	}

	private void WriteLength(int length)
	{
		binaryWriter.Write7BitEncodedInt(length);
	}

	private void WriteValue<T>(T value)
	{
		if (value is IBox bit) WriteObject(bit);
		else if (value is bool @bool) binaryWriter.Write(@bool);
		else if (value is char @char) binaryWriter.Write(@char);
		else if (value is byte @byte) binaryWriter.Write(@byte);
		else if (value is sbyte @sbyte) binaryWriter.Write(@sbyte);
		else if (value is short @short) binaryWriter.Write(@short);
		else if (value is ushort @ushort) binaryWriter.Write(@ushort);
		else if (value is int @int) binaryWriter.Write(@int);
		else if (value is uint @uint) binaryWriter.Write(@uint);
		else if (value is long @long) binaryWriter.Write(@long);
		else if (value is ulong @ulong) binaryWriter.Write(@ulong);
		else if (value is float @float) binaryWriter.Write(@float);
		else if (value is double @double) binaryWriter.Write(@double);
		else if (value is decimal @decimal) binaryWriter.Write(@decimal);
		else if (value is string @string) binaryWriter.Write(@string);
		else if (value is DateTime dateTime) binaryWriter.Write(dateTime.ToBinary());
		else if (value is TimeSpan timeSpan) binaryWriter.Write(timeSpan.Ticks);
		else throw new NotSupportedException(typeof(T).Name);
	}

	private void WriteObject(IBox box)
	{
		WriteChar('{');
		box.WriteTo(this);
		WriteChar('}');
	}
}