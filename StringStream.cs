using System;

namespace Box;

public class StringStream(string value)
{
	public int Position { get; private set; }
	public int Length => value.Length;

	public string ReadTo(char separator)
	{
		string result = value.Substring(Position, value.IndexOf(separator, Position) - Position);
		Position += result.Length + 1;
		return result;
	}

	public string Read(int length)
	{
		string result = value.Substring(Position, length);
		Position += length;
		return result;
	}

	public void Read(string expectedString)
	{
		if (value.Substring(Position, expectedString.Length) != expectedString) throw new Exception(value.Substring(Position - 10, 20));
		Position += expectedString.Length;
	}

	public char PeekChar()
	{
		return value[Position];
	}
}