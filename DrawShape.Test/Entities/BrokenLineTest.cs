using Xunit;

using System;
using System.IO;
using System.Windows.Media;
using System.Collections.Generic;

using DrawShape.Entities;

namespace DrawShape.Test.Entities
{
    /// <summary>
    /// Class to represent a test of broken line.
    /// </summary>
	public class BrokenLineTest
	{
		[Theory]
		[MemberData(nameof(ConstructorData.SuccessData), MemberType = typeof(ConstructorData))]
		public void TestConstructor(string inputName, List<Point> inputPoints, byte r, byte g, byte b, BrokenLine expectedBrokenLine)
		{
			var actualBrokenLine = new BrokenLine(inputName, inputPoints, new SolidColorBrush(Color.FromRgb(r, g, b)));
			Assert.Equal(expectedBrokenLine.Name, actualBrokenLine.Name);
			Assert.Equal(expectedBrokenLine.ColorBorder.R, actualBrokenLine.ColorBorder.R);
			Assert.Equal(expectedBrokenLine.ColorBorder.G, actualBrokenLine.ColorBorder.G);
			Assert.Equal(expectedBrokenLine.ColorBorder.B, actualBrokenLine.ColorBorder.B);
			Assert.NotNull(actualBrokenLine.Points);
			Assert.Equal(expectedBrokenLine.Points.Length, actualBrokenLine.Points.Length);
			for (var i = 0; i < actualBrokenLine.Points.Length; i++)
			{
				Assert.Equal(expectedBrokenLine.Points[i].X, actualBrokenLine.Points[i].X);
				Assert.Equal(expectedBrokenLine.Points[i].Y, actualBrokenLine.Points[i].Y);
			}
		}

		[Theory]
		[MemberData(nameof(ConstructorData.ThrowsInvalidDataExcpetionData), MemberType = typeof(ConstructorData))]
		public void TestConstructorThrowsInvalidDataExcpetion(List<Point> points)
		{
			Assert.Throws<InvalidDataException>(() => new BrokenLine("BrokenLine", points, Brushes.Violet));
		}

		[Fact]
		public void TestConstructorThrowsNullReferenceExcpetion()
		{
			Assert.Throws<NullReferenceException>(() => new BrokenLine("BrokenLine", null, Brushes.Violet));
		}

		private class ConstructorData
		{
			public static IEnumerable<object[]> SuccessData => new List<object[]>
			{
				new object[]
				{
                    "BrokenLine1", new List<Point>
					{
						new Point(1, 2), new Point(3, 4), new Point(5, 6),
						new Point(7, 8), new Point(9, 10), new Point(11, 12)
					},
					(byte)1, (byte)1, (byte)1,
					new BrokenLine(
                        "BrokenLine1",
                        new List<Point>
					    {
						    new Point(1, 2), new Point(3, 4), new Point(5, 6),
						    new Point(7, 8), new Point(9, 10), new Point(11, 12)
					    },
				    new SolidColorBrush(Color.FromRgb(1, 1, 1))) 
				},
				new object[]
				{
                    "BrokenLine2", new List<Point>
					{
						new Point(12, 11), new Point(10, 9), new Point(8, 7),
						new Point(6, 5), new Point(4, 3), new Point(2, 1)   
					},
					(byte)255, (byte)255, (byte)255,
					new BrokenLine(
                        "BrokenLine2",
                        new List<Point>
					    {
						    new Point(12, 11), new Point(10, 9), new Point(8, 7),
						    new Point(6, 5), new Point(4, 3), new Point(2, 1)
					    },
					new SolidColorBrush(Color.FromRgb(255, 255, 255)))
				},
				new object[]
				{
                    "BrokenLine3", new List<Point>
					{
						new Point(1.1, 2.2), new Point(3.3, 4.4), new Point(5.5, 6.6),
						new Point(7.7, 8.8), new Point(9.9, 10.10), new Point(11.11, 12.12)   
					},
					(byte)0, (byte)111, (byte)222,
					new BrokenLine(
                        "BrokenLine3",
                        new List<Point>
					    {
						    new Point(1.1, 2.2), new Point(3.3, 4.4), new Point(5.5, 6.6),
						    new Point(7.7, 8.8), new Point(9.9, 10.10), new Point(11.11, 12.12)
					    },
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
			Assert.Throws<NullReferenceException>(() => new BrokenLine().ToPolygon());
			Assert.Throws<InvalidDataException>(() => new BrokenLine("name", new List<Point>(), new SolidColorBrush()).ToPolygon());
		}
	}
}
