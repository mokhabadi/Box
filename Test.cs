using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Box;

public class Test
{
	private readonly bool @bool = true;
	private readonly char @char = 'M';
	private readonly byte @byte = 199;
	private readonly sbyte @sbyte = -77;
	private readonly short @short = 32000;
	private readonly ushort @ushort = 64000;
	private readonly int @int = -123456;
	private readonly uint @uint = 654321;
	private readonly long @long = -9876543210;
	private readonly ulong @ulong = 9123456789;
	private readonly float @float = 55.55f;
	private readonly double @double = 22.22;
	private readonly decimal @decimal = 66.66m;
	private readonly string @string = "Hello World";
	private readonly DateTime dateTime = new(2000, 1, 1);
	private readonly TimeSpan timeSpan = new(10, 10, 10);
	private readonly TestBox testBox = new(333, "test_box", new TestBox.InnerBox([4.4f, 4.5f, 4.6f]));

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

	public void Execute()
	{
		MemoryStream memoryStream = new();
		Write(memoryStream);
		memoryStream.Position = 0;
		byte[] bytes = memoryStream.ToArray();
		string box = string.Concat(bytes.Select(b => b is >= 32 and <= 126 ? ((char)b).ToString() : b.ToString()));
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
		
		writer.Write(@bool);
		writer.Write(@char);
		writer.Write(@byte);
		writer.Write(@sbyte);
		writer.Write(@short);
		writer.Write(@ushort);
		writer.Write(@int);
		writer.Write(@uint);
		writer.Write(@long);
		writer.Write(@ulong);
		writer.Write(@float);
		writer.Write(@double);
		writer.Write(@decimal);
		writer.Write(@string);
		writer.Write(dateTime);
		writer.Write(timeSpan);
		writer.Write(testBox);

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
	}

	private void Read(MemoryStream stream)
	{
		BinaryReader binaryReader = new(stream);
		IReader reader = new Reader(binaryReader);

		reader.Read(out bool @bool);
		reader.Read(out char @char);
		reader.Read(out byte @byte);
		reader.Read(out sbyte @sbyte);
		reader.Read(out short @short);
		reader.Read(out ushort @ushort);
		reader.Read(out int @int);
		reader.Read(out uint @uint);
		reader.Read(out long @long);
		reader.Read(out ulong @ulong);
		reader.Read(out float @float);
		reader.Read(out double @double);
		reader.Read(out decimal @decimal);
		reader.Read(out string @string);
		reader.Read(out DateTime dateTime);
		reader.Read(out TimeSpan timeSpan);
		reader.Read(out TestBox testBox);

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

		AssertEquality(this.@bool, @bool);
		AssertEquality(this.@char, @char);
		AssertEquality(this.@byte, @byte);
		AssertEquality(this.@sbyte, @sbyte);
		AssertEquality(this.@short, @short);
		AssertEquality(this.@ushort, @ushort);
		AssertEquality(this.@int, @int);
		AssertEquality(this.@uint, @uint);
		AssertEquality(this.@long, @long);
		AssertEquality(this.@ulong, @ulong);
		AssertEquality(this.@float, @float);
		AssertEquality(this.@double, @double);
		AssertEquality(this.@decimal, @decimal);
		AssertEquality(this.@string, @string);
		AssertEquality(this.dateTime, dateTime);
		AssertEquality(this.timeSpan, timeSpan);
		AssertEquality(this.testBox, testBox);

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