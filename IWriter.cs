using System.Runtime.CompilerServices;

namespace Box;

public interface IWriter
{
	public void Write<T>(T value, [CallerArgumentExpression(nameof(value))] string? key = null);
	public void Write<T>(T[] value, [CallerArgumentExpression(nameof(value))] string? key = null);
	public void WriteNullable<T>(T? value, [CallerArgumentExpression(nameof(value))] string? key = null);
	public void WriteNullable<T>(T[]? value, [CallerArgumentExpression(nameof(value))] string? key = null);
}