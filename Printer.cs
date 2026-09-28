using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Box;

public class Printer(BinaryReader binaryReader, StringBuilder stringBuilder)
{
	public string Print()
	{
		while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length) PrintItem();
		return stringBuilder.ToString();
	}

	private void PrintItem()
	{
		string type = binaryReader.ReadString();
		string key = binaryReader.ReadString();
		char valueSign = binaryReader.ReadChar();
		stringBuilder.Append($"{type} {key} {valueSign} ");
		PrintValue(type, valueSign);
		binaryReader.ReadChar();
	}

	private void PrintValue(string type, char valueSign)
	{
		if (valueSign == '~') stringBuilder.Append("null");
		else if (type == nameof(Boolean)) stringBuilder.Append(binaryReader.ReadBoolean());
		else if (type == nameof(Char)) stringBuilder.Append(binaryReader.ReadChar());
		else if (type == nameof(Byte)) stringBuilder.Append(binaryReader.ReadByte());
		else if (type == nameof(SByte)) stringBuilder.Append(binaryReader.ReadSByte());
		else if (type == nameof(Int16)) stringBuilder.Append(binaryReader.ReadInt16());
		else if (type == nameof(UInt16)) stringBuilder.Append(binaryReader.ReadUInt16());
		else if (type == nameof(Int32)) stringBuilder.Append(binaryReader.ReadInt32());
		else if (type == nameof(UInt32)) stringBuilder.Append(binaryReader.ReadUInt32());
		else if (type == nameof(Int64)) stringBuilder.Append(binaryReader.ReadInt64());
		else if (type == nameof(UInt64)) stringBuilder.Append(binaryReader.ReadUInt64());
		else if (type == nameof(Single)) stringBuilder.Append(binaryReader.ReadSingle().ToString(CultureInfo.InvariantCulture));
		else if (type == nameof(Double)) stringBuilder.Append(binaryReader.ReadDouble().ToString(CultureInfo.InvariantCulture));
		else if (type == nameof(Decimal)) stringBuilder.Append(binaryReader.ReadDecimal().ToString(CultureInfo.InvariantCulture));
		else if (type == nameof(DateTime)) stringBuilder.Append(DateTime.FromBinary(binaryReader.ReadInt64()).ToString("yyyy-MM-dd HH:mm:ss"));
		else if (type == nameof(TimeSpan)) stringBuilder.Append(TimeSpan.FromTicks(binaryReader.ReadInt64()).ToString());
		else if (type == nameof(String)) PrintString();
		else if (type.EndsWith("[]")) PrintArray(type[..^2]);
		else PrintObject();
		stringBuilder.Append(';');
	}

	private void PrintObject()
	{
		stringBuilder.Append(binaryReader.ReadChar());
		while (binaryReader.PeekChar() != '}') PrintItem();
		stringBuilder.Append(binaryReader.ReadChar());
	}

	private void PrintString()
	{
		string value = binaryReader.ReadString();
		stringBuilder.Append(value.Length);
		stringBuilder.Append('"');
		stringBuilder.Append(value);
		stringBuilder.Append('"');
	}

	private void PrintArray(string type)
	{
		int length = binaryReader.Read7BitEncodedInt();
		stringBuilder.Append(length);
		stringBuilder.Append('[');
		for (int i = 0; i < length; i++) PrintValue(type, '=');
		stringBuilder.Append(']');
	}

	public static string Print(byte[] bytes)
	{
		using MemoryStream memoryStream = new(bytes);
		using BinaryReader binaryReader = new(memoryStream);
		Printer printer = new(binaryReader, new StringBuilder());
		return printer.Print();
	}
}