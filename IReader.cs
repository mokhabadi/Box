using System.Runtime.CompilerServices;

namespace Box;

public interface IReader
{
	public void Read<T>(out T value, [CallerArgumentExpression(nameof(value))] string key = "");
	public void ReadNullable<T>(out T? value, [CallerArgumentExpression(nameof(value))] string key = "");
	public void Read<T>(out T[] value, [CallerArgumentExpression(nameof(value))] string key = "");
	public void ReadNullable<T>(out T[]? value, [CallerArgumentExpression(nameof(value))] string key = "");
}