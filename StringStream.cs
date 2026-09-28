namespace Box;

public class StringStream(string value)
{
	private int position;

	public bool Finished()
	{
		Trim();
		return position == value.Length;
	}

	public string ReadTo(string separator)
	{
		Trim();
		int from = position;
		while (!separator.Contains(value[position])) position++;
		int to = position;
		while (to > from && char.IsWhiteSpace(value[to - 1])) to--;
		string result = value.Substring(from, to - from);
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
		Trim();
		return value[position];
	}

	public void Trim()
	{
		while (position < value.Length && char.IsWhiteSpace(value[position])) position++;
	}
}