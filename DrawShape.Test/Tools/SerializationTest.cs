using Xunit;

using System;
using System.IO;
using System.Windows.Media;
using System.Collections.Generic;

using DrawShape.Tools;
using DrawShape.Entities;

namespace DrawShape.Test.Tools
{
    /// <summary>
	/// Class to represent a serialization.
	/// </summary>
	public class SerializationTest
	{
		[Theory]
		[MemberData(nameof(SerializationData.BrokenLinesData), MemberType = typeof(SerializationData))]
		public void TestSerializeBrokenLines(List<BrokenLine> inputBrokenLines, string file, string expected)
		{
			Serialization.SerializeBrokenLines(file, inputBrokenLines);
			var actual = File.ReadAllText(file);
			File.Delete(file);
			Assert.Equal(expected, actual);
		}
		
		[Theory]
		[MemberData(nameof(SerializationData.BrokenLinesData), MemberType = typeof(SerializationData))]
		public void TestDeserializeBrokenLines(List<BrokenLine> expectedBrokenLines, string file, string data)
		{
			File.WriteAllText(file, data);
			var actual = Serialization.DeserializeBrokenLines(file);
			File.Delete(file);
			Assert.Equal(expectedBrokenLines.Count, actual.Count);
			for (var i = 0; i < actual.Count; i++)
			{
				Assert.Equal(expectedBrokenLines[i].Name, actual[i].Name);
				Assert.Equal(expectedBrokenLines[i].ColorBorder.R, actual[i].ColorBorder.R);
				Assert.Equal(expectedBrokenLines[i].ColorBorder.G, actual[i].ColorBorder.G);
				Assert.Equal(expectedBrokenLines[i].ColorBorder.B, actual[i].ColorBorder.B);
				Assert.Equal(expectedBrokenLines[i].Points.Length, actual[i].Points.Length);
				for (var j = 0; j < actual[i].Points.Length; j++)
				{
					Assert.Equal(expectedBrokenLines[i].Points[j].X, actual[i].Points[j].X);
					Assert.Equal(expectedBrokenLines[i].Points[j].Y, actual[i].Points[j].Y);
				}
			}
		}

		private class SerializationData
		{
			public static IEnumerable<object[]> BrokenLinesData => new List<object[]>
			{
				new object[]
				{
					new List<BrokenLine>
					{
						new BrokenLine(
                            "BrokenLine1",
							new List<Point>
							{
								new Point(551.2, 74.2),
								new Point(592.8, 177.4),
								new Point(521.59999999999991, 343),
								new Point(351.2, 284.6),
								new Point(328, 147),
								new Point(360.8, 54.2),
							},
							new SolidColorBrush(Color.FromRgb(255, 0, 0))),
						new BrokenLine(
                            "BrokenLine2",
							new List<Point>
							{
								new Point(551.2, 74.2),
								new Point(592.8, 177.4),
								new Point(521.59999999999991, 343),
								new Point(351.2, 284.6),
								new Point(328, 147),
								new Point(360.8, 54.2),
							},
							new SolidColorBrush(Color.FromRgb(255, 0, 0))),
						new BrokenLine(
                            "BrokenLine3",
							new List<Point>
							{
								new Point(551.2, 74.2),
								new Point(592.8, 177.4),
								new Point(521.59999999999991, 343),
								new Point(351.2, 284.6),
								new Point(328, 147),
								new Point(360.8, 54.2),
							},
							new SolidColorBrush(Color.FromRgb(255, 0, 0)))
					},
					"TestFile.xml",
					"<?xml version=\"1.0\"?>" + Environment.NewLine +
                    "<ArrayOfBrokenLine xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" + Environment.NewLine +
                    "  <BrokenLine Name=\"BrokenLine1\">" + Environment.NewLine +
					"    <ColorBorder R=\"255\" G=\"0\" B=\"0\" />" + Environment.NewLine +
					"    <Points>" + Environment.NewLine +
					"      <Point X=\"551.2\" Y=\"74.2\" />" + Environment.NewLine +
					"      <Point X=\"592.8\" Y=\"177.4\" />" + Environment.NewLine +
					"      <Point X=\"521.59999999999991\" Y=\"343\" />" + Environment.NewLine +
					"      <Point X=\"351.2\" Y=\"284.6\" />" + Environment.NewLine +
					"      <Point X=\"328\" Y=\"147\" />" + Environment.NewLine +
					"      <Point X=\"360.8\" Y=\"54.2\" />" + Environment.NewLine +
					"    </Points>" + Environment.NewLine +
                    "  </BrokenLine>" + Environment.NewLine +
                    "  <BrokenLine Name=\"BrokenLine2\">" + Environment.NewLine +
					"    <ColorBorder R=\"255\" G=\"0\" B=\"0\" />" + Environment.NewLine +
					"    <Points>" + Environment.NewLine +
					"      <Point X=\"551.2\" Y=\"74.2\" />" + Environment.NewLine +
					"      <Point X=\"592.8\" Y=\"177.4\" />" + Environment.NewLine +
					"      <Point X=\"521.59999999999991\" Y=\"343\" />" + Environment.NewLine +
					"      <Point X=\"351.2\" Y=\"284.6\" />" + Environment.NewLine +
					"      <Point X=\"328\" Y=\"147\" />" + Environment.NewLine +
					"      <Point X=\"360.8\" Y=\"54.2\" />" + Environment.NewLine +
					"    </Points>" + Environment.NewLine +
                    "  </BrokenLine>" + Environment.NewLine +
                    "  <BrokenLine Name=\"BrokenLine3\">" + Environment.NewLine +
					"    <ColorBorder R=\"255\" G=\"0\" B=\"0\" />" + Environment.NewLine +
					"    <Points>" + Environment.NewLine +
					"      <Point X=\"551.2\" Y=\"74.2\" />" + Environment.NewLine +
					"      <Point X=\"592.8\" Y=\"177.4\" />" + Environment.NewLine +
					"      <Point X=\"521.59999999999991\" Y=\"343\" />" + Environment.NewLine +
					"      <Point X=\"351.2\" Y=\"284.6\" />" + Environment.NewLine +
					"      <Point X=\"328\" Y=\"147\" />" + Environment.NewLine +
					"      <Point X=\"360.8\" Y=\"54.2\" />" + Environment.NewLine +
					"    </Points>" + Environment.NewLine +
                    "  </BrokenLine>" + Environment.NewLine +
                    "</ArrayOfBrokenLine>"
                }
			};
		}
	}
}
