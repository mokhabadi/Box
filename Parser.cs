using System;
using System.IO;

namespace Box;

public class Parser(BinaryWriter binaryWriter, StringStream stringStream)
{
	public void Parse()
	{
		while (stringStream.Position < stringStream.Length) ParseItem();
	}

	private void ParseItem()
	{
		string type = stringStream.ReadTo(' ');
		string key = stringStream.ReadTo(' ');
		char valueSign = char.Parse(stringStream.ReadTo(' '));
		binaryWriter.Write(type);
		binaryWriter.Write(key);
		binaryWriter.Write(valueSign);
		ParseValue(type, valueSign);
		binaryWriter.Write(';');
	}

	private void ParseValue(string type, char valueSign)
	{
		if (valueSign == '~') stringStream.ReadTo(';');
		else if (type == nameof(Boolean)) binaryWriter.Write(bool.Parse(stringStream.ReadTo(';')));
		else if (type == nameof(Char)) binaryWriter.Write(char.Parse(stringStream.ReadTo(';')));
		else if (type == nameof(Byte)) binaryWriter.Write(byte.Parse(stringStream.ReadTo(';')));
		else if (type == nameof(SByte)) binaryWriter.Write(sbyte.Parse(stringStream.ReadTo(';')));
		else if (type == nameof(Int16)) binaryWriter.Write(short.Parse(stringStream.ReadTo(';')));
		else if (type == nameof(UInt16)) binaryWriter.Write(ushort.Parse(stringStream.ReadTo(';')));
		else if (type == nameof(Int32)) binaryWriter.Write(int.Parse(stringStream.ReadTo(';')));
		else if (type == nameof(UInt32)) binaryWriter.Write(uint.Parse(stringStream.ReadTo(';')));
		else if (type == nameof(Int64)) binaryWriter.Write(long.Parse(stringStream.ReadTo(';')));
		else if (type == nameof(UInt64)) binaryWriter.Write(ulong.Parse(stringStream.ReadTo(';')));
		else if (type == nameof(Single)) binaryWriter.Write(float.Parse(stringStream.ReadTo(';')));
		else if (type == nameof(Double)) binaryWriter.Write(double.Parse(stringStream.ReadTo(';')));
		else if (type == nameof(Decimal)) binaryWriter.Write(decimal.Parse(stringStream.ReadTo(';')));
		else if (type == nameof(String)) ParseString();
		else if (type == nameof(DateTime)) binaryWriter.Write(DateTime.Parse(stringStream.ReadTo(';')).ToBinary());
		else if (type == nameof(TimeSpan)) binaryWriter.Write(TimeSpan.Parse(stringStream.ReadTo(';')).Ticks);
		else if (type.EndsWith("[]")) ParseArray(type[..^2]);
		else ParseObject();
	}

	private void ParseString()
	{
		int length = int.Parse(stringStream.ReadTo('"'));
		string value = stringStream.Read(length);
		stringStream.Read("\";");
		binaryWriter.Write(value);
	}

	private void ParseArray(string type)
	{
		int length = int.Parse(stringStream.ReadTo('['));
		binaryWriter.Write7BitEncodedInt(length);
		for (int i = 0; i < length; i++) ParseValue(type, '=');
		stringStream.Read(2);
	}

	private void ParseObject()
	{
		stringStream.Read("{");
		binaryWriter.Write('{');
		while (stringStream.PeekChar() != '}') ParseItem();
		stringStream.Read("};");
		binaryWriter.Write('}');
	}
}