using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Box;

public class Reader(BinaryReader binaryReader) : IReader
{
	public T Read<T>(T? _, [CallerArgumentExpression(nameof(_))] string key = "")
	{
		Read(out T? value, key);
		return value!;
	}

	public void Read<T>(out T value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		string typeName = binaryReader.ReadString();
		string expectedKey = binaryReader.ReadString();
		Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
		key = key[(key.LastIndexOf(' ') + 1)..];
		if (key != expectedKey || type.Name != typeName) throw new Exception($"key/type mismatch '{type.Name} {key}', expected '{typeName} {expectedKey}'");
		char valueSign = binaryReader.ReadChar();
		if (valueSign is not ('=' or '~')) throw new Exception();
		value = (T)ReadValue(valueSign, type);
		if (binaryReader.ReadChar() != ';') throw new Exception();
	}

	private object ReadValue(char valueSign, Type type)
	{
		if (valueSign == '~') return null!;
		if (type.IsAssignableTo(typeof(IBox))) return ReadObject(type);
		if (type == typeof(bool)) return binaryReader.ReadBoolean();
		if (type == typeof(char)) return binaryReader.ReadChar();
		if (type == typeof(byte)) return binaryReader.ReadByte();
		if (type == typeof(sbyte)) return binaryReader.ReadSByte();
		if (type == typeof(short)) return binaryReader.ReadInt16();
		if (type == typeof(ushort)) return binaryReader.ReadUInt16();
		if (type == typeof(int)) return binaryReader.ReadInt32();
		if (type == typeof(uint)) return binaryReader.ReadUInt32();
		if (type == typeof(long)) return binaryReader.ReadInt64();
		if (type == typeof(ulong)) return binaryReader.ReadUInt64();
		if (type == typeof(float)) return binaryReader.ReadSingle();
		if (type == typeof(double)) return binaryReader.ReadDouble();
		if (type == typeof(decimal)) return binaryReader.ReadDecimal();
		if (type == typeof(string)) return binaryReader.ReadString();
		if (type == typeof(DateTime)) return DateTime.FromBinary(binaryReader.ReadInt64());
		if (type == typeof(TimeSpan)) return TimeSpan.FromTicks(binaryReader.ReadInt64());
		if (type == typeof(char[])) return ReadArray(binaryReader.ReadChars);
		if (type.IsAssignableTo(typeof(byte[]))) return ReadArray(binaryReader.ReadBytes);
		if (type.IsArray) return ReadArray(type.GetElementType()!);
		throw new NotSupportedException(type.FullName);
	}

	private object ReadObject(Type type)
	{
		if (binaryReader.ReadChar() != '{') throw new Exception();
		IBox box = (IBox)RuntimeHelpers.GetUninitializedObject(type);
		box.ReadFrom(this);
		if (binaryReader.ReadChar() != '}') throw new Exception();
		return box;
	}

	private T ReadArray<T>(Func<int, T> func)
	{
		int length = binaryReader.Read7BitEncodedInt();
		T array = func(length);
		return array;
	}

	private Array ReadArray(Type type)
	{
		int length = binaryReader.Read7BitEncodedInt();
		Array array = Array.CreateInstance(type, length);
		for (int i = 0; i < length; i++) array.SetValue(ReadValue('=', type), i);
		return array;
	}

	public static T Read<T>(T? _, byte[] bytes, [CallerArgumentExpression(nameof(_))] string key = "")
	{
		Read(out T value, bytes, key);
		return value;
	}

	public static void Read<T>(out T value, byte[] bytes, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		using MemoryStream memoryStream = new(bytes);
		using BinaryReader binaryReader = new(memoryStream);
		Reader reader = new(binaryReader);
		reader.Read(out value, key);
	}
}