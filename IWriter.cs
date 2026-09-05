using System.Runtime.CompilerServices;

namespace Box;

public interface IWriter
{
	public void Write<T>(T value, [CallerArgumentExpression(nameof(value))] string key = "");
	public void WriteNullable<T>(T? value, [CallerArgumentExpression(nameof(value))] string key = "");
	public bool Write<T>(T[] value, [CallerArgumentExpression(nameof(value))] string key = "");
	public bool WriteNullable<T>(T[]? value, [CallerArgumentExpression(nameof(value))] string key = "");
}