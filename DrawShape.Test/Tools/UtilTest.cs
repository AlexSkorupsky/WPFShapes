using Xunit;

using System.Windows.Media;
using System.Threading.Tasks;
using System.Collections.Generic;

using DrawShape.Tools;
using Point = DrawShape.Entities.Point;

namespace DrawShape.Test.Tools
{
	public class UtilTest
	{
		[Theory]
		[MemberData(nameof(ConstructorData.SuccessData), MemberType = typeof(ConstructorData))]
		public void TestOnSegment(System.Windows.Point p, System.Windows.Point q, System.Windows.Point r)
		{
			Assert.True(Util.OnSegment(p, q, r));
		}

		[Theory]
		[MemberData(nameof(ConstructorData.SuccessDataLine), MemberType = typeof(ConstructorData))]
		public void TestGetLine(Point start, Point end, Brush brush)
		{
			Task.Run(() =>
			{
				var line = Util.GetLine(start, end, brush);
				Assert.Equal(line.X1, start.X);
				Assert.Equal(line.Y1, start.Y);
				Assert.Equal(line.X2, end.X);
				Assert.Equal(line.Y2, end.Y);
				Assert.Equal(line.Fill, brush);
			});
		}

		private class ConstructorData
		{
			public static IEnumerable<object[]> SuccessData => new List<object[]>
			{
				new object[]
				{
				   new System.Windows.Point(10, 0),
				   new System.Windows.Point(18, 0),
				   new System.Windows.Point(30, 0)
				},
				new object[]
				{
					new System.Windows.Point(0, 10),
					new System.Windows.Point(0, 18),
					new System.Windows.Point(0, 30)
				},
				new object[]
				{
					new System.Windows.Point(-10, 10),
					new System.Windows.Point(0, 0),
					new System.Windows.Point(10, -30)
				},
				new object[]
				{
					new System.Windows.Point(-10, 10),
					new System.Windows.Point(5, 5),
					new System.Windows.Point(10, -30)
				}
			};

			public static IEnumerable<object[]> SuccessDataPoints => new List<object[]>
			{
				new object[]
				{
					new System.Windows.Point(0, 0),
					new System.Windows.Point(30, 0),
					new System.Windows.Point(2, 12),
					new System.Windows.Point(5, -12)
				},
				new object[]
				{
					new System.Windows.Point(-10, 0),
					new System.Windows.Point(30, 0),
					new System.Windows.Point(2, 12),
					new System.Windows.Point(5, -12)
				},
				new object[]
				{
					new System.Windows.Point(-12, 12),
					new System.Windows.Point(3, -12),
					new System.Windows.Point(12, 12),
					new System.Windows.Point(-12, -12)
				},
				new object[]
				{
					new System.Windows.Point(0, 0),
					new System.Windows.Point(30, 12),
					new System.Windows.Point(12, 12),
					new System.Windows.Point(15, -12)
				}
			};

			public static IEnumerable<object[]> SuccessDataLine => new List<object[]>
			{
				new object[]
				{
					new Point(10, 10),
					new Point(23, 17),
					new SolidColorBrush(Colors.Aqua) 
				},
				new object[]
				{
					new Point(0, 10),
					new Point(-23, 17),
					new SolidColorBrush(Colors.Aqua),
				},
				new object[]
				{
					new Point(1002, 340),
					new Point(54, 57),
					new SolidColorBrush(Colors.Aqua),
				},
				new object[]
				{
					new Point(102, 42),
					new Point(64, 71),
					new SolidColorBrush(Colors.Aqua),
				},
			};
		}
	}
}
