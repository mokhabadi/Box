using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Box;

public class Printer(BinaryReader binaryReader)
{
	public string Print()
	{
		StringBuilder result = new();
		
		while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
		{
			string[] lines = PrintItem().Split(Environment.NewLine);
			int indent = 0;

			foreach (string line in lines)
			{
				if (line.StartsWith('}')) indent--;
				result.Append(' ', indent * 4);
				if (line.StartsWith('{')) indent++;
				result.AppendLine(line);
			}
		}

		return result.ToString();
	}

	private string PrintItem()
	{
		bool isNullable = false;
		bool hasValue = true;
		bool isArray = false;
		int arrayLength = 0;
		int header = binaryReader.PeekChar();

		if (header is 'T' or 'F')
		{
			isNullable = true;
			hasValue = header is 'T';
			binaryReader.ReadByte();
			header = binaryReader.PeekChar();
		}

		if (header is 'A')
		{
			isArray = true;
			binaryReader.ReadByte();
			if (hasValue) arrayLength = binaryReader.Read7BitEncodedInt();
		}

		string typeName = binaryReader.ReadString();
		string key = binaryReader.ReadString();
		string value = hasValue ? isArray ? PrintArray(typeName, arrayLength) : PrintValue(typeName) : "null";
		string result = $"{typeName}{(isArray ? $"[{(hasValue ? arrayLength : "")}]" : "")}{(isNullable ? "?" : "")} {key} = {value};";
		return result;
	}

	private string PrintArray(string typeName, int arrayLength)
	{
		string result = "[";
		ReadChar('[');
		for (int i = 0; i < arrayLength; i++) result += PrintValue(typeName) + (i < arrayLength - 1 ? "," : "");
		result += "]";
		ReadChar(']');
		return result;
	}

	private string PrintValue(string typeName)
	{
		return typeName switch
		{
			"Boolean" => binaryReader.ReadBoolean().ToString(),
			"Char" => binaryReader.ReadChar().ToString(),
			"Byte" => binaryReader.ReadByte().ToString(),
			"SByte" => binaryReader.ReadSByte().ToString(),
			"Int16" => binaryReader.ReadInt16().ToString(),
			"UInt16" => binaryReader.ReadUInt16().ToString(),
			"Int32" => binaryReader.ReadInt32().ToString(),
			"UInt32" => binaryReader.ReadUInt32().ToString(),
			"Int64" => binaryReader.ReadInt64().ToString(),
			"UInt64" => binaryReader.ReadUInt64().ToString(),
			"Single" => binaryReader.ReadSingle().ToString(CultureInfo.InvariantCulture),
			"Double" => binaryReader.ReadDouble().ToString(CultureInfo.InvariantCulture),
			"Decimal" => binaryReader.ReadDecimal().ToString(CultureInfo.InvariantCulture),
			"String" => binaryReader.ReadString(),
			"DateTime" => DateTime.FromBinary(binaryReader.ReadInt64()).ToString(CultureInfo.InvariantCulture),
			"TimeSpan" => TimeSpan.FromTicks(binaryReader.ReadInt64()).ToString(),
			_ => PrintObject()
		};
	}

	private string PrintObject()
	{
		StringBuilder result = new();
		result.AppendLine();
		result.AppendLine("{");
		ReadChar('{');
		int value = binaryReader.PeekChar();

		while (value != '}')
		{
			result.AppendLine(PrintItem());
			value = binaryReader.PeekChar();
		}

		ReadChar('}');
		result.Append("}");
		return result.ToString();
	}


	private void ReadChar(char expectedValue)
	{
		char value = binaryReader.ReadChar();
		if (value != expectedValue) throw new Exception($"value mismatch '{value}', expected '{expectedValue}'");
	}
}