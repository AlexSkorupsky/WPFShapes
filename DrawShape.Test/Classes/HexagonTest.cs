using Xunit;

using System;
using System.IO;
using System.Windows.Media;
using System.Collections.Generic;

using DrawShape.Classes;

namespace DrawShape.Test.Classes
{
	public class HexagonTest
	{
		[Theory]
		[MemberData(nameof(ConstructorData.SuccessData), MemberType = typeof(ConstructorData))]
		public void TestConstructor(string inputName, List<Point> inputPoints, byte r, byte g, byte b, Hexagon expectedHexagon)
		{
			var actualHexagon = new Hexagon(inputName, inputPoints, new SolidColorBrush(Color.FromRgb(r, g, b)), new SolidColorBrush(Color.FromRgb(r, g, b)));
			Assert.Equal(expectedHexagon.Name, actualHexagon.Name);
			Assert.Equal(expectedHexagon.ColorFill.R, actualHexagon.ColorFill.R);
			Assert.Equal(expectedHexagon.ColorFill.G, actualHexagon.ColorFill.G);
			Assert.Equal(expectedHexagon.ColorFill.B, actualHexagon.ColorFill.B);
			Assert.Equal(expectedHexagon.ColorBorder.R, actualHexagon.ColorBorder.R);
			Assert.Equal(expectedHexagon.ColorBorder.G, actualHexagon.ColorBorder.G);
			Assert.Equal(expectedHexagon.ColorBorder.B, actualHexagon.ColorBorder.B);
			Assert.NotNull(actualHexagon.Points);
			Assert.Equal(expectedHexagon.Points.Length, actualHexagon.Points.Length);
			for (var i = 0; i < actualHexagon.Points.Length; i++)
			{
				Assert.Equal(expectedHexagon.Points[i].X, actualHexagon.Points[i].X);
				Assert.Equal(expectedHexagon.Points[i].Y, actualHexagon.Points[i].Y);
			}
		}

		[Theory]
		[MemberData(nameof(ConstructorData.ThrowsInvalidDataExcpetionData), MemberType = typeof(ConstructorData))]
		public void TestConstructorThrowsInvalidDataExcpetion(List<Point> points)
		{
			Assert.Throws<InvalidDataException>(() => new Hexagon("Hexagon", points, Brushes.Violet, Brushes.Violet));
		}

		[Fact]
		public void TestConstructorThrowsNullReferenceExcpetion()
		{
			Assert.Throws<NullReferenceException>(() => new Hexagon("Hexagon", null, Brushes.Violet, Brushes.Violet));
		}

		private class ConstructorData
		{
			public static IEnumerable<object[]> SuccessData => new List<object[]>
			{
				new object[]
				{
					"Hexagon 1", new List<Point>
					{
						new Point(1, 2), new Point(3, 4), new Point(5, 6),
						new Point(7, 8), new Point(9, 10), new Point(11, 12)
					},
					(byte)1, (byte)1, (byte)1,
					new Hexagon(
                        "Hexagon 1",
                        new List<Point>
					    {
						    new Point(1, 2), new Point(3, 4), new Point(5, 6),
						    new Point(7, 8), new Point(9, 10), new Point(11, 12)
					    },
                    new SolidColorBrush(Color.FromRgb(1, 1, 1)),
				    new SolidColorBrush(Color.FromRgb(1, 1, 1))) 
				},
				new object[]
				{
					"Hexagon 2", new List<Point>
					{
						new Point(12, 11), new Point(10, 9), new Point(8, 7),
						new Point(6, 5), new Point(4, 3), new Point(2, 1)   
					},
					(byte)255, (byte)255, (byte)255,
					new Hexagon(
                        "Hexagon 2",
                        new List<Point>
					    {
						    new Point(12, 11), new Point(10, 9), new Point(8, 7),
						    new Point(6, 5), new Point(4, 3), new Point(2, 1)
					    },
                    new SolidColorBrush(Color.FromRgb(255, 255, 255)),
					new SolidColorBrush(Color.FromRgb(255, 255, 255)))
				},
				new object[]
				{
					"Super Hexagon", new List<Point>
					{
						new Point(1.1, 2.2), new Point(3.3, 4.4), new Point(5.5, 6.6),
						new Point(7.7, 8.8), new Point(9.9, 10.10), new Point(11.11, 12.12)   
					},
					(byte)0, (byte)111, (byte)222,
					new Hexagon(
                        "Super Hexagon",
                        new List<Point>
					    {
						    new Point(1.1, 2.2), new Point(3.3, 4.4), new Point(5.5, 6.6),
						    new Point(7.7, 8.8), new Point(9.9, 10.10), new Point(11.11, 12.12)
					    },
                    new SolidColorBrush(Color.FromRgb(0, 111, 222)),
					new SolidColorBrush(Color.FromRgb(0, 111, 222)))
				}
			};

			public static IEnumerable<object[]> ThrowsInvalidDataExcpetionData => new List<object[]>
			{
				new object[]
				{
					new List<Point>()	
				},
				new object[]
				{
					new List<Point>
					{
						new Point(1, 2)
					}, 	
				},
				new object[]
				{
					new List<Point>
					{
						new Point(1, 2), new Point(3, 4), new Point(5, 6),
						new Point(7, 8), new Point(9, 10)
					}, 	
				},
				new object[]
				{
					new List<Point>
					{
						new Point(1, 2), new Point(3, 4), new Point(5, 6),
						new Point(7, 8), new Point(9, 10), new Point(11, 12), new Point(123, 321)
					}
				}
			};
		}

		[Fact]
		public void TestToPolygonThrows()
		{
			Assert.Throws<NullReferenceException>(() => new Hexagon().ToPolygon());
			Assert.Throws<InvalidDataException>(() => new Hexagon("name", new List<Point>(), new SolidColorBrush(), new SolidColorBrush()).ToPolygon());
		}
	}
}
