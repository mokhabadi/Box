namespace Box;

public interface IBox
{
	public void WriteTo(IWriter writer);
	public void ReadFrom(IReader reader);
}