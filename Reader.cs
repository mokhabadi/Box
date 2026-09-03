using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Box;

public class Reader(BinaryReader binaryReader) : IReader
{
	public void Read<T>(out T value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		ReadTypeAndKey<T>(key);
		ReadValue(out value);
	}

	public void ReadNullable<T>(out T? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		bool hasValue = ReadHasValue();
		ReadTypeAndKey<T>(key);
		if (hasValue) ReadValue(out value);
		else value = default;
	}

	public void Read<T>(out T[] value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		ReadChar('A');
		int length = ReadLength();
		ReadTypeAndKey<T>(key);
		value = new T[length];
		ReadChar('[');
		for (int i = 0; i < length; i++) ReadValue(out value[i]);
		ReadChar(']');
	}

	public void ReadNullable<T>(out T[]? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		bool hasValue = ReadHasValue();
		ReadChar('A');
		int length = hasValue ? ReadLength() : 0;
		value = hasValue ? new T[length] : null;
		ReadTypeAndKey<T>(key);
		if (value == null) return;
		ReadChar('[');
		for (int i = 0; i < value.Length; i++) ReadValue(out value[i]);
		ReadChar(']');
	}

	private void ReadTypeAndKey<T>(string expectedKey)
	{
		Type type = GetType<T>();
		string expectedTypeName = type.Name;
		string typeName = binaryReader.ReadString();
		if (typeName != expectedTypeName) throw new Exception($"type mismatch '{typeName}', expected '{expectedTypeName}'");
		string key = binaryReader.ReadString();
		expectedKey = expectedKey[(expectedKey.LastIndexOf(' ') + 1)..];
		if (key != expectedKey) throw new Exception($"value mismatch '{key}', expected '{expectedKey}'");
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

	private void ReadValue<T>(out T value)
	{
		Type type = GetType<T>();
		if (type.IsAssignableTo(typeof(IBox))) ReadObject(out value);
		else if (type == typeof(bool)) value = (T)(object)binaryReader.ReadBoolean();
		else if (type == typeof(char)) value = (T)(object)binaryReader.ReadChar();
		else if (type == typeof(byte)) value = (T)(object)binaryReader.ReadByte();
		else if (type == typeof(sbyte)) value = (T)(object)binaryReader.ReadSByte();
		else if (type == typeof(short)) value = (T)(object)binaryReader.ReadInt16();
		else if (type == typeof(ushort)) value = (T)(object)binaryReader.ReadUInt16();
		else if (type == typeof(int)) value = (T)(object)binaryReader.ReadInt32();
		else if (type == typeof(uint)) value = (T)(object)binaryReader.ReadUInt32();
		else if (type == typeof(long)) value = (T)(object)binaryReader.ReadInt64();
		else if (type == typeof(ulong)) value = (T)(object)binaryReader.ReadUInt64();
		else if (type == typeof(float)) value = (T)(object)binaryReader.ReadSingle();
		else if (type == typeof(double)) value = (T)(object)binaryReader.ReadDouble();
		else if (type == typeof(decimal)) value = (T)(object)binaryReader.ReadDecimal();
		else if (type == typeof(string)) value = (T)(object)binaryReader.ReadString();
		else if (type == typeof(DateTime)) value = (T)(object)DateTime.FromBinary(binaryReader.ReadInt64());
		else if (type == typeof(TimeSpan)) value = (T)(object)TimeSpan.FromTicks(binaryReader.ReadInt64());
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