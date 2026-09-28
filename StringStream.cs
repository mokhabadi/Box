namespace Box;

public class StringStream(string value)
{
	private int position;

	public bool Finished()
	{
		return position == value.Length;
	}

	public void Trim()
	{
		while (position < value.Length && char.IsWhiteSpace(value[position])) position++;
	}

	public string ReadTo(string separator)
	{
		Trim();
		int from = position;
		while (!separator.Contains(value[position])) position++;
		string result = value.Substring(from, position - from);
		return result;
	}

	public string Read(int length)
	{
		string result = value.Substring(position, length);
		position += length;
		return result;
	}

	public char ReadChar()
	{
		char result = value[position];
		position++;
		return result;
	}

	public char PeekChar()
	{
		return value[position];
	}
}