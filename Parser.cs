using System;
using System.IO;

namespace Box;

public class Parser(BinaryWriter binaryWriter, StringStream stringStream)
{
	public void Parse()
	{
		while (!stringStream.Finished()) ParseItem();
	}

	private void ParseItem()
	{
		string type = stringStream.ReadTo(" ");
		string key = stringStream.ReadTo("=~");
		char valueSign = stringStream.ReadChar();
		binaryWriter.Write(type);
		binaryWriter.Write(key);
		binaryWriter.Write(valueSign);
		ParseValue(type, valueSign);
		binaryWriter.Write(';');
	}

	private void ParseValue(string type, char valueSign)
	{
		if (valueSign == '~') stringStream.ReadTo(";");
		else if (type == nameof(Boolean)) binaryWriter.Write(bool.Parse(stringStream.ReadTo(";")));
		else if (type == nameof(Char)) binaryWriter.Write(char.Parse(stringStream.ReadTo(";")));
		else if (type == nameof(Byte)) binaryWriter.Write(byte.Parse(stringStream.ReadTo(";")));
		else if (type == nameof(SByte)) binaryWriter.Write(sbyte.Parse(stringStream.ReadTo(";")));
		else if (type == nameof(Int16)) binaryWriter.Write(short.Parse(stringStream.ReadTo(";")));
		else if (type == nameof(UInt16)) binaryWriter.Write(ushort.Parse(stringStream.ReadTo(";")));
		else if (type == nameof(Int32)) binaryWriter.Write(int.Parse(stringStream.ReadTo(";")));
		else if (type == nameof(UInt32)) binaryWriter.Write(uint.Parse(stringStream.ReadTo(";")));
		else if (type == nameof(Int64)) binaryWriter.Write(long.Parse(stringStream.ReadTo(";")));
		else if (type == nameof(UInt64)) binaryWriter.Write(ulong.Parse(stringStream.ReadTo(";")));
		else if (type == nameof(Single)) binaryWriter.Write(float.Parse(stringStream.ReadTo(";")));
		else if (type == nameof(Double)) binaryWriter.Write(double.Parse(stringStream.ReadTo(";")));
		else if (type == nameof(Decimal)) binaryWriter.Write(decimal.Parse(stringStream.ReadTo(";")));
		else if (type == nameof(DateTime)) binaryWriter.Write(DateTime.Parse(stringStream.ReadTo(";")).ToBinary());
		else if (type == nameof(TimeSpan)) binaryWriter.Write(TimeSpan.Parse(stringStream.ReadTo(";")).Ticks);
		else if (type == nameof(String)) ParseString();
		else if (type.EndsWith("[]")) ParseArray(type[..^2]);
		else ParseObject();
		stringStream.ReadChar();
		stringStream.Trim();
	}

	private void ParseString()
	{
		int length = int.Parse(stringStream.ReadTo("\""));
		stringStream.ReadChar();
		string value = stringStream.Read(length);
		stringStream.ReadTo(";");
		binaryWriter.Write(value);
	}

	private void ParseArray(string type)
	{
		int length = int.Parse(stringStream.ReadTo("["));
		stringStream.ReadChar();
		binaryWriter.Write7BitEncodedInt(length);
		for (int i = 0; i < length; i++) ParseValue(type, '=');
		stringStream.ReadTo(";");
	}

	private void ParseObject()
	{
		stringStream.ReadTo("{");
		stringStream.ReadChar();
		binaryWriter.Write('{');
		while (stringStream.PeekChar() != '}') ParseItem();
		stringStream.ReadTo(";");
		binaryWriter.Write('}');
	}

	public static byte[] Parse(string value)
	{
		using MemoryStream memoryStream = new();
		using BinaryWriter binaryWriter = new(memoryStream);
		StringStream stringStream = new(value.Trim());
		Parser parser = new(binaryWriter, stringStream);
		parser.Parse();
		return memoryStream.ToArray();
	}
}