using Xunit;

using System;
using System.IO;
using System.Windows.Media;
using System.Collections.Generic;

using DrawShape.Utils;
using DrawShape.Classes;

namespace DrawShape.Test.Utils
{
	public class SerializationTest
	{
		[Theory]
		[MemberData(nameof(SerializationData.HexagonsData), MemberType = typeof(SerializationData))]
		public void TestSerializeHexagons(List<Hexagon> inputHexagons, string file, string expected)
		{
			Serialization.SerializeHexagons(file, inputHexagons);
			var actual = File.ReadAllText(file);
			File.Delete(file);
			Assert.Equal(expected, actual);
		}
		
		[Theory]
		[MemberData(nameof(SerializationData.HexagonsData), MemberType = typeof(SerializationData))]
		public void TestDeserializeHexagons(List<Hexagon> expectedHexagons, string file, string data)
		{
			File.WriteAllText(file, data);
			var actual = Serialization.DeserializeHexagons(file);
			File.Delete(file);
			Assert.Equal(expectedHexagons.Count, actual.Count);
			for (var i = 0; i < actual.Count; i++)
			{
				Assert.Equal(expectedHexagons[i].Name, actual[i].Name);
				Assert.Equal(expectedHexagons[i].ColorFill.R, actual[i].ColorFill.R);
				Assert.Equal(expectedHexagons[i].ColorFill.G, actual[i].ColorFill.G);
				Assert.Equal(expectedHexagons[i].ColorFill.B, actual[i].ColorFill.B);
				Assert.Equal(expectedHexagons[i].ColorBorder.R, actual[i].ColorBorder.R);
				Assert.Equal(expectedHexagons[i].ColorBorder.G, actual[i].ColorBorder.G);
				Assert.Equal(expectedHexagons[i].ColorBorder.B, actual[i].ColorBorder.B);
				Assert.Equal(expectedHexagons[i].Points.Length, actual[i].Points.Length);
				for (var j = 0; j < actual[i].Points.Length; j++)
				{
					Assert.Equal(expectedHexagons[i].Points[j].X, actual[i].Points[j].X);
					Assert.Equal(expectedHexagons[i].Points[j].Y, actual[i].Points[j].Y);
				}
			}
		}

		private class SerializationData
		{
			public static IEnumerable<object[]> HexagonsData => new List<object[]>
			{
				new object[]
				{
					new List<Hexagon>
					{
						new Hexagon(
							"Hexagon0",
							new List<Point>
							{
								new Point(551.2, 74.2),
								new Point(592.8, 177.4),
								new Point(521.59999999999991, 343),
								new Point(351.2, 284.6),
								new Point(328, 147),
								new Point(360.8, 54.2),
							},
							new SolidColorBrush(Color.FromRgb(0, 255, 0)),
							new SolidColorBrush(Color.FromRgb(255, 0, 0))),
						new Hexagon(
							"Hexagon0",
							new List<Point>
							{
								new Point(551.2, 74.2),
								new Point(592.8, 177.4),
								new Point(521.59999999999991, 343),
								new Point(351.2, 284.6),
								new Point(328, 147),
								new Point(360.8, 54.2),
							},
							new SolidColorBrush(Color.FromRgb(0, 255, 0)),
							new SolidColorBrush(Color.FromRgb(255, 0, 0))),
						new Hexagon(
							"Hexagon0",
							new List<Point>
							{
								new Point(551.2, 74.2),
								new Point(592.8, 177.4),
								new Point(521.59999999999991, 343),
								new Point(351.2, 284.6),
								new Point(328, 147),
								new Point(360.8, 54.2),
							},
							new SolidColorBrush(Color.FromRgb(0, 255, 0)),
							new SolidColorBrush(Color.FromRgb(255, 0, 0)))
					},
					"TestFile.xml",
					"<?xml version=\"1.0\"?>" + Environment.NewLine +
					"<ArrayOfHexagon xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" + Environment.NewLine +
					"  <Hexagon Name=\"Hexagon0\">" + Environment.NewLine +
					"    <ColorFill R=\"0\" G=\"255\" B=\"0\" />" + Environment.NewLine +
					"    <ColorBorder R=\"255\" G=\"0\" B=\"0\" />" + Environment.NewLine +
					"    <Points>" + Environment.NewLine +
					"      <Point X=\"551.2\" Y=\"74.2\" />" + Environment.NewLine +
					"      <Point X=\"592.8\" Y=\"177.4\" />" + Environment.NewLine +
					"      <Point X=\"521.59999999999991\" Y=\"343\" />" + Environment.NewLine +
					"      <Point X=\"351.2\" Y=\"284.6\" />" + Environment.NewLine +
					"      <Point X=\"328\" Y=\"147\" />" + Environment.NewLine +
					"      <Point X=\"360.8\" Y=\"54.2\" />" + Environment.NewLine +
					"    </Points>" + Environment.NewLine +
					"  </Hexagon>" + Environment.NewLine +
					"  <Hexagon Name=\"Hexagon0\">" + Environment.NewLine +
					"    <ColorFill R=\"0\" G=\"255\" B=\"0\" />" + Environment.NewLine +
					"    <ColorBorder R=\"255\" G=\"0\" B=\"0\" />" + Environment.NewLine +
					"    <Points>" + Environment.NewLine +
					"      <Point X=\"551.2\" Y=\"74.2\" />" + Environment.NewLine +
					"      <Point X=\"592.8\" Y=\"177.4\" />" + Environment.NewLine +
					"      <Point X=\"521.59999999999991\" Y=\"343\" />" + Environment.NewLine +
					"      <Point X=\"351.2\" Y=\"284.6\" />" + Environment.NewLine +
					"      <Point X=\"328\" Y=\"147\" />" + Environment.NewLine +
					"      <Point X=\"360.8\" Y=\"54.2\" />" + Environment.NewLine +
					"    </Points>" + Environment.NewLine +
					"  </Hexagon>" + Environment.NewLine +
					"  <Hexagon Name=\"Hexagon0\">" + Environment.NewLine +
					"    <ColorFill R=\"0\" G=\"255\" B=\"0\" />" + Environment.NewLine +
					"    <ColorBorder R=\"255\" G=\"0\" B=\"0\" />" + Environment.NewLine +
					"    <Points>" + Environment.NewLine +
					"      <Point X=\"551.2\" Y=\"74.2\" />" + Environment.NewLine +
					"      <Point X=\"592.8\" Y=\"177.4\" />" + Environment.NewLine +
					"      <Point X=\"521.59999999999991\" Y=\"343\" />" + Environment.NewLine +
					"      <Point X=\"351.2\" Y=\"284.6\" />" + Environment.NewLine +
					"      <Point X=\"328\" Y=\"147\" />" + Environment.NewLine +
					"      <Point X=\"360.8\" Y=\"54.2\" />" + Environment.NewLine +
					"    </Points>" + Environment.NewLine +
					"  </Hexagon>" + Environment.NewLine +
					"</ArrayOfHexagon>"
				}
			};
		}
	}
}
