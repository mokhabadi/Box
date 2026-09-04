using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Box;

public class Reader(BinaryReader binaryReader) : IReader
{
	public void Read<T>(out T value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		value = Read(default(T), key);
	}

	public T Read<T>(T? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		string typeName = ReadTypeAndKey<T>(key);
		value = ReadValue<T>(typeName);
		return value;
	}

	public void ReadNullable<T>(out T? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		value = ReadNullable(default(T), key);
	}

	public T? ReadNullable<T>(T? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		bool hasValue = ReadHasValue();
		string typeName = ReadTypeAndKey<T>(key);
		value = hasValue ? ReadValue<T>(typeName) : default;
		return value;
	}

	public void Read<T>(out T[] value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		value = Read(default(T[]), key);
	}

	public T[] Read<T>(T[]? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		ReadChar('A');
		int length = ReadLength();
		string typeName = ReadTypeAndKey<T>(key);
		value = new T[length];
		ReadChar('[');
		for (int i = 0; i < length; i++) value[i] = ReadValue<T>(typeName);
		ReadChar(']');
		return value;
	}

	public void ReadNullable<T>(out T[]? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		value = ReadNullable(default(T[]), key);
	}

	public T[]? ReadNullable<T>(T[]? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		bool hasValue = ReadHasValue();
		ReadChar('A');
		int length = hasValue ? ReadLength() : 0;
		value = hasValue ? new T[length] : null;
		string typeName = ReadTypeAndKey<T>(key);
		if (value == null) return null;
		ReadChar('[');
		for (int i = 0; i < value.Length; i++) value[i] = ReadValue<T>(typeName);
		ReadChar(']');
		return value;
	}

	private string ReadTypeAndKey<T>(string expectedKey)
	{
		Type type = GetType<T>();
		string expectedTypeName = type.Name;
		string typeName = binaryReader.ReadString();
		if (typeName != expectedTypeName) throw new Exception($"type mismatch '{typeName}', expected '{expectedTypeName}'");
		string key = binaryReader.ReadString();
		expectedKey = expectedKey[(expectedKey.LastIndexOf(' ') + 1)..];
		if (key != expectedKey) throw new Exception($"value mismatch '{key}', expected '{expectedKey}'");
		if (type.IsAssignableTo(typeof(IBox))) typeName = nameof(IBox);
		return typeName;
	}

	private Type GetType<T>()
	{
		Type type = typeof(T);
		Type? nullableType = Nullable.GetUnderlyingType(type);
		return nullableType ?? type;
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

	private T ReadValue<T>(string typeName)
	{
		return typeName switch
		{
			nameof(Boolean) => (T)(object)binaryReader.ReadBoolean(),
			nameof(Char) => (T)(object)binaryReader.ReadChar(),
			nameof(Byte) => (T)(object)binaryReader.ReadByte(),
			nameof(SByte) => (T)(object)binaryReader.ReadSByte(),
			nameof(Int16) => (T)(object)binaryReader.ReadInt16(),
			nameof(UInt16) => (T)(object)binaryReader.ReadUInt16(),
			nameof(Int32) => (T)(object)binaryReader.ReadInt32(),
			nameof(UInt32) => (T)(object)binaryReader.ReadUInt32(),
			nameof(Int64) => (T)(object)binaryReader.ReadInt64(),
			nameof(UInt64) => (T)(object)binaryReader.ReadUInt64(),
			nameof(Single) => (T)(object)binaryReader.ReadSingle(),
			nameof(Double) => (T)(object)binaryReader.ReadDouble(),
			nameof(Decimal) => (T)(object)binaryReader.ReadDecimal(),
			nameof(String) => (T)(object)binaryReader.ReadString(),
			nameof(DateTime) => (T)(object)DateTime.FromBinary(binaryReader.ReadInt64()),
			nameof(TimeSpan) => (T)(object)TimeSpan.FromTicks(binaryReader.ReadInt64()),
			nameof(IBox) => ReadObject<T>(),
			_ => throw new ArgumentOutOfRangeException(nameof(typeName), typeName, null)
		};
	}

	private T ReadObject<T>()
	{
		ReadChar('{');
		T value = (T)RuntimeHelpers.GetUninitializedObject(typeof(T));
		((IBox)value).ReadFrom(this);
		ReadChar('}');
		return value;
	}
}