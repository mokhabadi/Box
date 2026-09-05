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

	public void ReadNullable<T>(out T? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		value = ReadNullable(default(T), key);
	}

	public void Read<T>(out T[] value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		value = Read(default(T[]), key);
	}

	public void ReadNullable<T>(out T[]? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		value = ReadNullable(default(T[]), key);
	}

	public T Read<T>(T? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		string typeName = ReadTypeAndKey<T>(key);
		value = ReadValue<T>(typeName);
		ReadChar('|');
		return value;
	}

	public T? ReadNullable<T>(T? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		bool hasValue = ReadHasValue();
		if (hasValue) return Read(value, key);
		ReadTypeAndKey<T>(key);
		ReadChar('|');
		return value;
	}

	public T[] Read<T>(T[]? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		ReadChar('A');
		int length = binaryReader.Read7BitEncodedInt();
		string typeName = ReadTypeAndKey<T>(key);
		value = new T[length];
		ReadChar('[');
		for (int i = 0; i < length; i++) value[i] = ReadValue<T>(typeName);
		ReadChar(']');
		ReadChar('|');
		return value;
	}

	public T[]? ReadNullable<T>(T[]? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		bool hasValue = ReadHasValue();
		if (hasValue) return Read(value, key);
		ReadChar('A');
		ReadTypeAndKey<T>(key);
		ReadChar('|');
		return null;
	}

	private string ReadTypeAndKey<T>(string expectedKey)
	{
		Type type = typeof(T);
		type = Nullable.GetUnderlyingType(type) ?? type;
		if (type.IsAssignableTo(typeof(IBox))) type = typeof(object);
		string expectedTypeName = type.Name;
		string typeName = binaryReader.ReadString();
		if (typeName != expectedTypeName) throw new Exception($"type mismatch '{typeName}', expected '{expectedTypeName}'");
		string key = binaryReader.ReadString();
		expectedKey = expectedKey[(expectedKey.LastIndexOf(' ') + 1)..];
		if (key != expectedKey) throw new Exception($"value mismatch '{key}', expected '{expectedKey}'");
		if (type.IsAssignableTo(typeof(IBox))) typeName = nameof(IBox);
		return typeName;
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
			nameof(Object) => ReadObject<T>(),
			_ => throw new NotSupportedException(typeName)
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