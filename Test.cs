using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Box;

public class Test
{
	private readonly bool boolValue = true;
	private readonly char charValue = 'M';
	private readonly byte byteValue = 199;
	private readonly sbyte sbyteValue = -77;
	private readonly short shortValue = 32000;
	private readonly ushort ushortValue = 64000;
	private readonly int intValue = -123456;
	private readonly uint uintValue = 654321;
	private readonly long longValue = -9876543210;
	private readonly ulong ulongValue = 9123456789;
	private readonly float floatValue = 55.55f;
	private readonly double doubleValue = 22.22;
	private readonly decimal decimalValue = 66.66m;
	private readonly string stringValue = "Hello World";
	private readonly DateTime dateTimeValue = new(2000, 1, 1);
	private readonly TimeSpan timeSpanValue = new(10, 10, 10);
	private readonly TestBox testBoxValue = new(333, "test_box", new TestBox.InnerBox([4.4f, 4.5f, 4.6f]));

	private readonly bool? boolNull = null;
	private readonly char? charNull = null;
	private readonly byte? byteNull = null;
	private readonly sbyte? sbyteNull = null;
	private readonly short? shortNull = null;
	private readonly ushort? ushortNull = null;
	private readonly int? intNull = null;
	private readonly uint? uintNull = null;
	private readonly long? longNull = null;
	private readonly ulong? ulongNull = null;
	private readonly float? floatNull = null;
	private readonly double? doubleNull = null;
	private readonly decimal? decimalNull = null;
	private readonly string? stringNull = null;
	private readonly DateTime? dateTimeNull = null;
	private readonly TimeSpan? timeSpanNull = null;
	private readonly TestBox? testBoxNull = null;

	private readonly bool? boolNonNull =  true;
	private readonly char? charNonNull =  'M';
	private readonly byte? byteNonNull =  199;
	private readonly sbyte? sbyteNonNull =  -77;
	private readonly short? shortNonNull =  32000;
	private readonly ushort? ushortNonNull =  64000;
	private readonly int? intNonNull =  -123456;
	private readonly uint? uintNonNull =  654321;
	private readonly long? longNonNull =  -9876543210;
	private readonly ulong? ulongNonNull =  9123456789;
	private readonly float? floatNonNull =  55.55f;
	private readonly double? doubleNonNull =  22.22;
	private readonly decimal? decimalNonNull =  66.66m;
	private readonly string? stringNonNull =  "Hello World";
	private readonly DateTime? dateTimeNonNull =  new(2000, 1, 1);
	private readonly TimeSpan? timeSpanNonNull =  new(10, 10, 10);
	private readonly TestBox? testBoxNonNull =  new(333, "test_box", new TestBox.InnerBox([4.4f, 4.5f, 4.6f]));

	private readonly bool[] boolArray = [true, false, true, false];
	private readonly char[] charArray = ['A', 'B', 'C', 'D', 'E', 'F'];
	private readonly byte[] byteArray = [10, 20, 30, 40];
	private readonly sbyte[] sbyteArray = [-10, -20, -30, -40];
	private readonly short[] shortArray = [1000, 10000];
	private readonly ushort[] ushortArray = [6000, 60000];
	private readonly int[] intArray = [999999, -99];
	private readonly uint[] uintArray = [uint.MaxValue, 43];
	private readonly long[] longArray = [10123000123, -23];
	private readonly ulong[] ulongArray = [ulong.MaxValue, 31];
	private readonly float[] floatArray = [1.1f, 2.2f];
	private readonly double[] doubleArray = [0.123, 0.456];
	private readonly decimal[] decimalArray = [9.99m, 7.99m];
	private readonly string[] stringArray = ["alice", "bob"];
	private readonly DateTime[] dateTimeArray = [new(2026, 6, 6), new(2027, 7, 7)];
	private readonly TimeSpan[] timeSpanArray = [TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(30)];
	private readonly TestBox[] testBoxArray = [new TestBox(1, "one", new TestBox.InnerBox([1, 1, 1])), new(2, "two", new TestBox.InnerBox([2, 2, 2]))];

	private readonly bool[]? boolNullArray = null;
	private readonly char[]? charNullArray = null;
	private readonly byte[]? byteNullArray = null;
	private readonly sbyte[]? sbyteNullArray = null;
	private readonly short[]? shortNullArray = null;
	private readonly ushort[]? ushortNullArray = null;
	private readonly int[]? intNullArray = null;
	private readonly uint[]? uintNullArray = null;
	private readonly long[]? longNullArray = null;
	private readonly ulong[]? ulongNullArray = null;
	private readonly float[]? floatNullArray = null;
	private readonly double[]? doubleNullArray = null;
	private readonly decimal[]? decimalNullArray = null;
	private readonly string[]? stringNullArray = null;
	private readonly DateTime[]? dateTimeNullArray = null;
	private readonly TimeSpan[]? timeSpanNullArray = null;
	private readonly TestBox[]? testBoxNullArray = null;

	private readonly bool[]? boolNonNullArray = [true, false, true, false];
	private readonly char[]? charNonNullArray = ['A', 'B', 'C', 'D', 'E', 'F'];
	private readonly byte[]? byteNonNullArray = [10, 20, 30, 40];
	private readonly sbyte[]? sbyteNonNullArray = [-10, -20, -30, -40];
	private readonly short[]? shortNonNullArray = [1000, 10000];
	private readonly ushort[]? ushortNonNullArray = [6000, 60000];
	private readonly int[]? intNonNullArray = [999999, -99];
	private readonly uint[]? uintNonNullArray = [uint.MaxValue, 43];
	private readonly long[]? longNonNullArray = [10123000123, -23];
	private readonly ulong[]? ulongNonNullArray = [ulong.MaxValue, 31];
	private readonly float[]? floatNonNullArray = [1.1f, 2.2f];
	private readonly double[]? doubleNonNullArray = [0.123, 0.456];
	private readonly decimal[]? decimalNonNullArray = [9.99m, 7.99m];
	private readonly string[]? stringNonNullArray = ["alice", "bob"];
	private readonly DateTime[]? dateTimeNonNullArray = [new(2026, 6, 6), new(2027, 7, 7)];
	private readonly TimeSpan[]? timeSpanNonNullArray = [TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(30)];
	private readonly TestBox[]? testBoxNonNullArray = [new TestBox(1, "one", new TestBox.InnerBox([1, 1, 1])), new(2, "two", new TestBox.InnerBox([2, 2, 2]))];

	public void Execute()
	{
		MemoryStream memoryStream = new();
		Write(memoryStream);
		memoryStream.Position = 0;
		byte[] bytes = memoryStream.ToArray();
		string box = string.Concat(bytes.Select(b => b is >= 32 and <= 126 ? $"{(char)b}" : $"{b}"));
		Trace.WriteLine(box);
		Printer printer = new(new BinaryReader(memoryStream));
		string print = printer.Print();
		Trace.WriteLine(print);
		memoryStream.Position = 0;
		Read(memoryStream);
	}

	private void Write(MemoryStream stream)
	{
		BinaryWriter binaryWriter = new(stream);
		IWriter writer = new Writer(binaryWriter);
		
		writer.Write(boolValue);
		writer.Write(charValue);
		writer.Write(byteValue);
		writer.Write(sbyteValue);
		writer.Write(shortValue);
		writer.Write(ushortValue);
		writer.Write(intValue);
		writer.Write(uintValue);
		writer.Write(longValue);
		writer.Write(ulongValue);
		writer.Write(floatValue);
		writer.Write(doubleValue);
		writer.Write(decimalValue);
		writer.Write(stringValue);
		writer.Write(dateTimeValue);
		writer.Write(timeSpanValue);
		writer.Write(testBoxValue);

		writer.WriteNullable(boolNull);
		writer.WriteNullable(charNull);
		writer.WriteNullable(byteNull);
		writer.WriteNullable(sbyteNull);
		writer.WriteNullable(shortNull);
		writer.WriteNullable(ushortNull);
		writer.WriteNullable(intNull);
		writer.WriteNullable(uintNull);
		writer.WriteNullable(longNull);
		writer.WriteNullable(ulongNull);
		writer.WriteNullable(floatNull);
		writer.WriteNullable(doubleNull);
		writer.WriteNullable(decimalNull);
		writer.WriteNullable(stringNull);
		writer.WriteNullable(dateTimeNull);
		writer.WriteNullable(timeSpanNull);
		writer.WriteNullable(testBoxNull);

		writer.WriteNullable(boolNonNull);
		writer.WriteNullable(charNonNull);
		writer.WriteNullable(byteNonNull);
		writer.WriteNullable(sbyteNonNull);
		writer.WriteNullable(shortNonNull);
		writer.WriteNullable(ushortNonNull);
		writer.WriteNullable(intNonNull);
		writer.WriteNullable(uintNonNull);
		writer.WriteNullable(longNonNull);
		writer.WriteNullable(ulongNonNull);
		writer.WriteNullable(floatNonNull);
		writer.WriteNullable(doubleNonNull);
		writer.WriteNullable(decimalNonNull);
		writer.WriteNullable(stringNonNull);
		writer.WriteNullable(dateTimeNonNull);
		writer.WriteNullable(timeSpanNonNull);
		writer.WriteNullable(testBoxNonNull);

		writer.Write(boolArray);
		writer.Write(charArray);
		writer.Write(byteArray);
		writer.Write(sbyteArray);
		writer.Write(shortArray);
		writer.Write(ushortArray);
		writer.Write(intArray);
		writer.Write(uintArray);
		writer.Write(longArray);
		writer.Write(ulongArray);
		writer.Write(floatArray);
		writer.Write(doubleArray);
		writer.Write(decimalArray);
		writer.Write(stringArray);
		writer.Write(dateTimeArray);
		writer.Write(timeSpanArray);
		writer.Write(testBoxArray);

		writer.WriteNullable(boolNullArray);
		writer.WriteNullable(charNullArray);
		writer.WriteNullable(byteNullArray);
		writer.WriteNullable(sbyteNullArray);
		writer.WriteNullable(shortNullArray);
		writer.WriteNullable(ushortNullArray);
		writer.WriteNullable(intNullArray);
		writer.WriteNullable(uintNullArray);
		writer.WriteNullable(longNullArray);
		writer.WriteNullable(ulongNullArray);
		writer.WriteNullable(floatNullArray);
		writer.WriteNullable(doubleNullArray);
		writer.WriteNullable(decimalNullArray);
		writer.WriteNullable(stringNullArray);
		writer.WriteNullable(dateTimeNullArray);
		writer.WriteNullable(timeSpanNullArray);
		writer.WriteNullable(testBoxNullArray);

		writer.WriteNullable(boolNonNullArray);
		writer.WriteNullable(charNonNullArray);
		writer.WriteNullable(byteNonNullArray);
		writer.WriteNullable(sbyteNonNullArray);
		writer.WriteNullable(shortNonNullArray);
		writer.WriteNullable(ushortNonNullArray);
		writer.WriteNullable(intNonNullArray);
		writer.WriteNullable(uintNonNullArray);
		writer.WriteNullable(longNonNullArray);
		writer.WriteNullable(ulongNonNullArray);
		writer.WriteNullable(floatNonNullArray);
		writer.WriteNullable(doubleNonNullArray);
		writer.WriteNullable(decimalNonNullArray);
		writer.WriteNullable(stringNonNullArray);
		writer.WriteNullable(dateTimeNonNullArray);
		writer.WriteNullable(timeSpanNonNullArray);
		writer.WriteNullable(testBoxNonNullArray);
	}

	private void Read(MemoryStream stream)
	{
		BinaryReader binaryReader = new(stream);
		IReader reader = new Reader(binaryReader);

		reader.Read(out bool boolValue);
		reader.Read(out char charValue);
		reader.Read(out byte byteValue);
		reader.Read(out sbyte sbyteValue);
		reader.Read(out short shortValue);
		reader.Read(out ushort ushortValue);
		reader.Read(out int intValue);
		reader.Read(out uint uintValue);
		reader.Read(out long longValue);
		reader.Read(out ulong ulongValue);
		reader.Read(out float floatValue);
		reader.Read(out double doubleValue);
		reader.Read(out decimal decimalValue);
		reader.Read(out string stringValue);
		reader.Read(out DateTime dateTimeValue);
		reader.Read(out TimeSpan timeSpanValue);
		reader.Read(out TestBox testBoxValue);

		reader.ReadNullable(out bool? boolNull);
		reader.ReadNullable(out char? charNull);
		reader.ReadNullable(out byte? byteNull);
		reader.ReadNullable(out sbyte? sbyteNull);
		reader.ReadNullable(out short? shortNull);
		reader.ReadNullable(out ushort? ushortNull);
		reader.ReadNullable(out int? intNull);
		reader.ReadNullable(out uint? uintNull);
		reader.ReadNullable(out long? longNull);
		reader.ReadNullable(out ulong? ulongNull);
		reader.ReadNullable(out float? floatNull);
		reader.ReadNullable(out double? doubleNull);
		reader.ReadNullable(out decimal? decimalNull);
		reader.ReadNullable(out string? stringNull);
		reader.ReadNullable(out DateTime? dateTimeNull);
		reader.ReadNullable(out TimeSpan? timeSpanNull);
		reader.ReadNullable(out TestBox? testBoxNull);

		reader.ReadNullable(out bool? boolNonNull);
		reader.ReadNullable(out char? charNonNull);
		reader.ReadNullable(out byte? byteNonNull);
		reader.ReadNullable(out sbyte? sbyteNonNull);
		reader.ReadNullable(out short? shortNonNull);
		reader.ReadNullable(out ushort? ushortNonNull);
		reader.ReadNullable(out int? intNonNull);
		reader.ReadNullable(out uint? uintNonNull);
		reader.ReadNullable(out long? longNonNull);
		reader.ReadNullable(out ulong? ulongNonNull);
		reader.ReadNullable(out float? floatNonNull);
		reader.ReadNullable(out double? doubleNonNull);
		reader.ReadNullable(out decimal? decimalNonNull);
		reader.ReadNullable(out string? stringNonNull);
		reader.ReadNullable(out DateTime? dateTimeNonNull);
		reader.ReadNullable(out TimeSpan? timeSpanNonNull);
		reader.ReadNullable(out TestBox? testBoxNonNull);

		reader.Read(out bool[] boolArray);
		reader.Read(out char[] charArray);
		reader.Read(out byte[] byteArray);
		reader.Read(out sbyte[] sbyteArray);
		reader.Read(out short[] shortArray);
		reader.Read(out ushort[] ushortArray);
		reader.Read(out int[] intArray);
		reader.Read(out uint[] uintArray);
		reader.Read(out long[] longArray);
		reader.Read(out ulong[] ulongArray);
		reader.Read(out float[] floatArray);
		reader.Read(out double[] doubleArray);
		reader.Read(out decimal[] decimalArray);
		reader.Read(out string[] stringArray);
		reader.Read(out DateTime[] dateTimeArray);
		reader.Read(out TimeSpan[] timeSpanArray);
		reader.Read(out TestBox[] testBoxArray);

		reader.ReadNullable(out bool[]? boolNullArray);
		reader.ReadNullable(out char[]? charNullArray);
		reader.ReadNullable(out byte[]? byteNullArray);
		reader.ReadNullable(out sbyte[]? sbyteNullArray);
		reader.ReadNullable(out short[]? shortNullArray);
		reader.ReadNullable(out ushort[]? ushortNullArray);
		reader.ReadNullable(out int[]? intNullArray);
		reader.ReadNullable(out uint[]? uintNullArray);
		reader.ReadNullable(out long[]? longNullArray);
		reader.ReadNullable(out ulong[]? ulongNullArray);
		reader.ReadNullable(out float[]? floatNullArray);
		reader.ReadNullable(out double[]? doubleNullArray);
		reader.ReadNullable(out decimal[]? decimalNullArray);
		reader.ReadNullable(out string[]? stringNullArray);
		reader.ReadNullable(out DateTime[]? dateTimeNullArray);
		reader.ReadNullable(out TimeSpan[]? timeSpanNullArray);
		reader.ReadNullable(out TestBox[]? testBoxNullArray);

		reader.ReadNullable(out bool[]? boolNonNullArray);
		reader.ReadNullable(out char[]? charNonNullArray);
		reader.ReadNullable(out byte[]? byteNonNullArray);
		reader.ReadNullable(out sbyte[]? sbyteNonNullArray);
		reader.ReadNullable(out short[]? shortNonNullArray);
		reader.ReadNullable(out ushort[]? ushortNonNullArray);
		reader.ReadNullable(out int[]? intNonNullArray);
		reader.ReadNullable(out uint[]? uintNonNullArray);
		reader.ReadNullable(out long[]? longNonNullArray);
		reader.ReadNullable(out ulong[]? ulongNonNullArray);
		reader.ReadNullable(out float[]? floatNonNullArray);
		reader.ReadNullable(out double[]? doubleNonNullArray);
		reader.ReadNullable(out decimal[]? decimalNonNullArray);
		reader.ReadNullable(out string[]? stringNonNullArray);
		reader.ReadNullable(out DateTime[]? dateTimeNonNullArray);
		reader.ReadNullable(out TimeSpan[]? timeSpanNonNullArray);
		reader.ReadNullable(out TestBox[]? testBoxNonNullArray);

		AssertEquality(this.boolValue, boolValue);
		AssertEquality(this.charValue, charValue);
		AssertEquality(this.byteValue, byteValue);
		AssertEquality(this.sbyteValue, sbyteValue);
		AssertEquality(this.shortValue, shortValue);
		AssertEquality(this.ushortValue, ushortValue);
		AssertEquality(this.intValue, intValue);
		AssertEquality(this.uintValue, uintValue);
		AssertEquality(this.longValue, longValue);
		AssertEquality(this.ulongValue, ulongValue);
		AssertEquality(this.floatValue, floatValue);
		AssertEquality(this.doubleValue, doubleValue);
		AssertEquality(this.decimalValue, decimalValue);
		AssertEquality(this.stringValue, stringValue);
		AssertEquality(this.dateTimeValue, dateTimeValue);
		AssertEquality(this.timeSpanValue, timeSpanValue);
		AssertEquality(this.testBoxValue, testBoxValue);

		AssertEquality(this.boolNull, boolNull);
		AssertEquality(this.charNull, charNull);
		AssertEquality(this.byteNull, byteNull);
		AssertEquality(this.sbyteNull, sbyteNull);
		AssertEquality(this.shortNull, shortNull);
		AssertEquality(this.ushortNull, ushortNull);
		AssertEquality(this.intNull, intNull);
		AssertEquality(this.uintNull, uintNull);
		AssertEquality(this.longNull, longNull);
		AssertEquality(this.ulongNull, ulongNull);
		AssertEquality(this.floatNull, floatNull);
		AssertEquality(this.doubleNull, doubleNull);
		AssertEquality(this.decimalNull, decimalNull);
		AssertEquality(this.stringNull, stringNull);
		AssertEquality(this.dateTimeNull, dateTimeNull);
		AssertEquality(this.timeSpanNull, timeSpanNull);
		AssertEquality(this.testBoxNull, testBoxNull);

		AssertEquality(this.boolNonNull, boolNonNull);
		AssertEquality(this.charNonNull, charNonNull);
		AssertEquality(this.byteNonNull, byteNonNull);
		AssertEquality(this.sbyteNonNull, sbyteNonNull);
		AssertEquality(this.shortNonNull, shortNonNull);
		AssertEquality(this.ushortNonNull, ushortNonNull);
		AssertEquality(this.intNonNull, intNonNull);
		AssertEquality(this.uintNonNull, uintNonNull);
		AssertEquality(this.longNonNull, longNonNull);
		AssertEquality(this.ulongNonNull, ulongNonNull);
		AssertEquality(this.floatNonNull, floatNonNull);
		AssertEquality(this.doubleNonNull, doubleNonNull);
		AssertEquality(this.decimalNonNull, decimalNonNull);
		AssertEquality(this.stringNonNull, stringNonNull);
		AssertEquality(this.dateTimeNonNull, dateTimeNonNull);
		AssertEquality(this.timeSpanNonNull, timeSpanNonNull);
		AssertEquality(this.testBoxNonNull, testBoxNonNull);

		AssertEquality(this.boolArray, boolArray);
		AssertEquality(this.charArray, charArray);
		AssertEquality(this.byteArray, byteArray);
		AssertEquality(this.sbyteArray, sbyteArray);
		AssertEquality(this.shortArray, shortArray);
		AssertEquality(this.ushortArray, ushortArray);
		AssertEquality(this.intArray, intArray);
		AssertEquality(this.uintArray, uintArray);
		AssertEquality(this.longArray, longArray);
		AssertEquality(this.ulongArray, ulongArray);
		AssertEquality(this.floatArray, floatArray);
		AssertEquality(this.doubleArray, doubleArray);
		AssertEquality(this.decimalArray, decimalArray);
		AssertEquality(this.stringArray, stringArray);
		AssertEquality(this.dateTimeArray, dateTimeArray);
		AssertEquality(this.timeSpanArray, timeSpanArray);
		AssertEquality(this.testBoxArray, testBoxArray);

		AssertEquality(this.boolNullArray, boolNullArray);
		AssertEquality(this.charNullArray, charNullArray);
		AssertEquality(this.byteNullArray, byteNullArray);
		AssertEquality(this.sbyteNullArray, sbyteNullArray);
		AssertEquality(this.shortNullArray, shortNullArray);
		AssertEquality(this.ushortNullArray, ushortNullArray);
		AssertEquality(this.intNullArray, intNullArray);
		AssertEquality(this.uintNullArray, uintNullArray);
		AssertEquality(this.longNullArray, longNullArray);
		AssertEquality(this.ulongNullArray, ulongNullArray);
		AssertEquality(this.floatNullArray, floatNullArray);
		AssertEquality(this.doubleNullArray, doubleNullArray);
		AssertEquality(this.decimalNullArray, decimalNullArray);
		AssertEquality(this.stringNullArray, stringNullArray);
		AssertEquality(this.dateTimeNullArray, dateTimeNullArray);
		AssertEquality(this.timeSpanNullArray, timeSpanNullArray);
		AssertEquality(this.testBoxNullArray, testBoxNullArray);

		AssertEquality(this.boolNonNullArray, boolNonNullArray);
		AssertEquality(this.charNonNullArray, charNonNullArray);
		AssertEquality(this.byteNonNullArray, byteNonNullArray);
		AssertEquality(this.sbyteNonNullArray, sbyteNonNullArray);
		AssertEquality(this.shortNonNullArray, shortNonNullArray);
		AssertEquality(this.ushortNonNullArray, ushortNonNullArray);
		AssertEquality(this.intNonNullArray, intNonNullArray);
		AssertEquality(this.uintNonNullArray, uintNonNullArray);
		AssertEquality(this.longNonNullArray, longNonNullArray);
		AssertEquality(this.ulongNonNullArray, ulongNonNullArray);
		AssertEquality(this.floatNonNullArray, floatNonNullArray);
		AssertEquality(this.doubleNonNullArray, doubleNonNullArray);
		AssertEquality(this.decimalNonNullArray, decimalNonNullArray);
		AssertEquality(this.stringNonNullArray, stringNonNullArray);
		AssertEquality(this.dateTimeNonNullArray, dateTimeNonNullArray);
		AssertEquality(this.timeSpanNonNullArray, timeSpanNonNullArray);
		AssertEquality(this.testBoxNonNullArray, testBoxNonNullArray);
	}

	private static void AssertEquality<T>(T t1, T t2)
	{
		if (!Equals(t1, t2)) throw new Exception($"value mismatch '{t1}' and '{t2}'");
	}

	private static void AssertEquality<T>(T[]? t1, T[]? t2)
	{
		if (t1 == t2) return;
		if (t1 == null || t2 == null || !t1.SequenceEqual(t2)) throw new Exception($"value mismatch '{t1}' and '{t2}'");
	}

	private class TestBox : IBox
	{
		private int id;
		private string name;
		private InnerBox innerBox;

		public TestBox(int id, string name, InnerBox innerBox)
		{
			this.id = id;
			this.name = name;
			this.innerBox = innerBox;
		}

		public void WriteTo(IWriter writer)
		{
			writer.Write(id);
			writer.Write(name);
			writer.Write(innerBox);
		}

		public void ReadFrom(IReader reader)
		{
			reader.Read(out id);
			reader.Read(out name);
			reader.Read(out innerBox);
		}

		public override bool Equals(object? obj)
		{
			return obj is TestBox testBox && id == testBox.id && name == testBox.name && innerBox.Equals(testBox.innerBox);
		}

		public override int GetHashCode() => 0;

		public class InnerBox : IBox
		{
			private float[] dimensions;

			public InnerBox(float[] dimensions)
			{
				this.dimensions = dimensions;
			}

			public void WriteTo(IWriter writer)
			{
				writer.Write(dimensions);
			}

			public void ReadFrom(IReader reader)
			{
				reader.Read(out dimensions);
			}

			public override bool Equals(object? obj)
			{
				return obj is InnerBox other && dimensions.SequenceEqual(other.dimensions);
			}

			public override int GetHashCode() => 0;
		}
	}
}