using System.Runtime.CompilerServices;

namespace Box;

public class Writer(BinaryWriter binaryWriter) : IWriter
{
	public void Write<T>(T value, [CallerArgumentExpression(nameof(value))] string? key = null)
	{
		WriteType<T>();
		WriteKey(key);
		WriteValue(value);
	}

	public void Write<T>(T[] value, [CallerArgumentExpression(nameof(value))] string? key = null)
	{
		WriteChar('A');
		WriteLength(value.Length);
		WriteType<T>();
		WriteKey(key);
		WriteChar('[');
		foreach (T item in value) WriteValue(item);
		WriteChar(']');
	}

	public void WriteNullable<T>(T? value, [CallerArgumentExpression(nameof(value))] string? key = null)
	{
		WriteHasValue(value);
		WriteType<T>();
		WriteKey(key);
		if (value != null) WriteValue(value);
	}

	public void WriteNullable<T>(T[]? value, [CallerArgumentExpression(nameof(value))] string? key = null)
	{
		WriteHasValue(value);
		WriteChar('A');
		if (value != null) WriteLength(value.Length);
		WriteType<T>();
		WriteKey(key);
		if (value == null) return;
		WriteChar('[');
		foreach (T item in value) WriteValue(item);
		WriteChar(']');
	}

	private void WriteKey(string? key)
	{
		if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key is null or empty", nameof(key));
		binaryWriter.Write(key);
	}

	private void WriteType<T>()
	{
		char typeId = typeof(T).GetTypeId();
		binaryWriter.Write(typeId);
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
		else throw new NotSupportedException(typeof(T).Name);
	}

	private void WriteObject(IBox box)
	{
		WriteChar('{');
		box.WriteTo(this);
		WriteChar('}');
	}
}