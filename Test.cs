using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Box;

public class Test
{
	private bool @bool = true;
	private char @char = 'M';
	private byte @byte = 199;
	private sbyte @sbyte = -77;
	private short @short = 32000;
	private ushort @ushort = 64000;
	private int @int = -123456;
	private uint @uint = 654321;
	private long @long = -9876543210;
	private ulong @ulong = 9123456789;
	private float @float = 55.55f;
	private double @double = 22.22;
	private decimal @decimal = 66.66m;
	private string @string = "Hello World";
	private DateTime dateTime = new(2000, 1, 1);
	private TimeSpan timeSpan = new(10, 10, 10);
	private TestBox testBox = new(333, "test_box", new TestBox.InnerBox([4.4f, 4.5f, 4.6f]));

	private bool? boolNull = null;
	private char? charNull = null;
	private byte? byteNull = null;
	private sbyte? sbyteNull = null;
	private short? shortNull = null;
	private ushort? ushortNull = null;
	private int? intNull = null;
	private uint? uintNull = null;
	private long? longNull = null;
	private ulong? ulongNull = null;
	private float? floatNull = null;
	private double? doubleNull = null;
	private decimal? decimalNull = null;
	private string? stringNull = null;
	private DateTime? dateTimeNull = null;
	private TimeSpan? timeSpanNull = null;
	private TestBox? testBoxNull = null;

	private bool[] boolArray = [true, false, true, false];
	private char[] charArray = ['A', 'B', 'C', 'D', 'E', 'F'];
	private byte[] byteArray = [10, 20, 30, 40];
	private sbyte[] sbyteArray = [-10, -20, -30, -40];
	private short[] shortArray = [1000, 10000];
	private ushort[] ushortArray = [6000, 60000];
	private int[] intArray = [999999, -99];
	private uint[] uintArray = [uint.MaxValue, 43];
	private long[] longArray = [10123000123, -23];
	private ulong[] ulongArray = [ulong.MaxValue, 31];
	private float[] floatArray = [1.1f, 2.2f];
	private double[] doubleArray = [0.123, 0.456];
	private decimal[] decimalArray = [9.99m, 7.99m];
	private string[] stringArray = ["alice", "bob"];
	private DateTime[] dateTimeArray = [new(2026, 6, 6), new(2027, 7, 7)];
	private TimeSpan[] timeSpanArray = [TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(30)];
	private TestBox[] testBoxArray = [new TestBox(1, "one", new TestBox.InnerBox([1, 1, 1])), new(2, "two", new TestBox.InnerBox([2, 2, 2]))];

	private bool[]? boolNullArray = null;
	private char[]? charNullArray = null;
	private byte[]? byteNullArray = null;
	private sbyte[]? sbyteNullArray = null;
	private short[]? shortNullArray = null;
	private ushort[]? ushortNullArray = null;
	private int[]? intNullArray = null;
	private uint[]? uintNullArray = null;
	private long[]? longNullArray = null;
	private ulong[]? ulongNullArray = null;
	private float[]? floatNullArray = null;
	private double[]? doubleNullArray = null;
	private decimal[]? decimalNullArray = null;
	private string[]? stringNullArray = null;
	private DateTime[]? dateTimeNullArray = null;
	private TimeSpan[]? timeSpanNullArray = null;
	private TestBox[]? testBoxNullArray = null;

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
		Writer writer = new(binaryWriter);
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
		Reader reader = new(binaryReader);

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
	}

	private void AssertEquality<T>(T t1, T t2)
	{
		if (!Equals(t1, t2)) throw new Exception($"value mismatch '{t1}' and '{t2}'");
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
			return obj is TestBox testBox && id == testBox.id && name == testBox.name;
		}

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
		}
	}
}