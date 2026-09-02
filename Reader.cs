using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Box;

public class Reader(BinaryReader binaryReader) : IReader
{
	public void Read<T>(out T value, [CallerArgumentExpression(nameof(value))] string? key = null)
	{
		ReadKey(key);
		ReadType<T>();
		ReadValue(out value);
	}

	public void Read<T>(out T[] value, [CallerArgumentExpression(nameof(value))] string? key = null)
	{
		ReadKey(key);
		ReadChar('A');
		int length = ReadLength();
		ReadType<T>();
		value = new T[length];
		ReadChar('[');
		for (int i = 0; i < length; i++) ReadValue(out value[i]);
		ReadChar(']');
	}

	public void ReadNullable<T>(out T? value, [CallerArgumentExpression(nameof(value))] string? key = null)
	{
		ReadKey(key);
		bool hasValue = ReadHasValue();
		ReadType<T>();
		if (hasValue) ReadValue(out value);
		else value = default;
	}

	public void ReadNullable<T>(out T[]? value, [CallerArgumentExpression(nameof(value))] string? key = null)
	{
		ReadKey(key);
		bool hasValue = ReadHasValue();
		ReadChar('A');
		int length = hasValue ? ReadLength() : 0;
		value = hasValue ? new T[length] : null;
		ReadType<T>();
		if (value == null) return;
		ReadChar('[');
		for (int i = 0; i < value.Length; i++) ReadValue(out value[i]);
		ReadChar(']');
	}

	private void ReadKey(string? expectedKey)
	{
		if (string.IsNullOrWhiteSpace(expectedKey)) throw new ArgumentException("key is null or empty", nameof(expectedKey));
		ReadChar('|');
		string key = binaryReader.ReadString();
		expectedKey = expectedKey[(expectedKey.LastIndexOf(' ') + 1)..];
		if (key != expectedKey) throw new Exception($"value mismatch '{key}', expected '{expectedKey}'");
	}

	private void ReadType<T>()
	{
		Type type = typeof(T);
		Type? nullableType = Nullable.GetUnderlyingType(type);
		string expectedTypeName = nullableType != null ? nullableType.Name : type.Name;
		string typeName = binaryReader.ReadString();
		if (typeName != expectedTypeName) throw new Exception($"type mismatch '{typeName}', expected '{expectedTypeName}'");
	}

	private void ReadChar(char expectedValue)
	{
		char value = binaryReader.ReadChar();
		if (value != expectedValue) throw new Exception($"value mismatch '{value}', expected '{expectedValue}'");
	}

	private bool ReadHasValue()
	{
		char hasValue = binaryReader.ReadChar();
		if (hasValue == 'T') return true;
		if (hasValue == 'F') return false;
		throw new Exception($"undefined value state: '{hasValue}'");
	}

	private int ReadLength()
	{
		int length = binaryReader.Read7BitEncodedInt();
		return length;
	}

	private void ReadValue<T>(out T value)
	{
		if (typeof(T).IsAssignableTo(typeof(IBox))) ReadObject(out value);
		else if (typeof(T) == typeof(bool)) value = (T)(object)binaryReader.ReadBoolean();
		else if (typeof(T) == typeof(char)) value = (T)(object)binaryReader.ReadChar();
		else if (typeof(T) == typeof(byte)) value = (T)(object)binaryReader.ReadByte();
		else if (typeof(T) == typeof(sbyte)) value = (T)(object)binaryReader.ReadSByte();
		else if (typeof(T) == typeof(short)) value = (T)(object)binaryReader.ReadInt16();
		else if (typeof(T) == typeof(ushort)) value = (T)(object)binaryReader.ReadUInt16();
		else if (typeof(T) == typeof(int)) value = (T)(object)binaryReader.ReadInt32();
		else if (typeof(T) == typeof(uint)) value = (T)(object)binaryReader.ReadUInt32();
		else if (typeof(T) == typeof(long)) value = (T)(object)binaryReader.ReadInt64();
		else if (typeof(T) == typeof(ulong)) value = (T)(object)binaryReader.ReadUInt64();
		else if (typeof(T) == typeof(float)) value = (T)(object)binaryReader.ReadSingle();
		else if (typeof(T) == typeof(double)) value = (T)(object)binaryReader.ReadDouble();
		else if (typeof(T) == typeof(decimal)) value = (T)(object)binaryReader.ReadDecimal();
		else if (typeof(T) == typeof(string)) value = (T)(object)binaryReader.ReadString();
		else if (typeof(T) == typeof(DateTime)) value = (T)(object)DateTime.FromBinary(binaryReader.ReadInt64());
		else if (typeof(T) == typeof(TimeSpan)) value = (T)(object)TimeSpan.FromTicks(binaryReader.ReadInt64());
		else throw new NotSupportedException(typeof(T).Name);
	}

	private void ReadObject<T>(out T value)
	{
		ReadChar('{');
		value = (T)RuntimeHelpers.GetUninitializedObject(typeof(T));
		((IBox)value).ReadFrom(this);
		ReadChar('}');
	}
}