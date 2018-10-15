using Xunit;

using System.Collections.Generic;

using DrawShape.Classes;

namespace DrawShape.Test.Classes
{
	public class PointTest
	{
		[Theory]
		[MemberData(nameof(ConstructorData.SuccessData), MemberType = typeof(ConstructorData))]
		public void TestConstructor(double inputX, double inputY, Point expectedPoint)
		{
			var actual = new Point(inputX, inputY);
			Assert.Equal(actual.X, expectedPoint.X);
			Assert.Equal(actual.Y, expectedPoint.Y);
		}

		private class ConstructorData
		{
			public static IEnumerable<object[]> SuccessData => new List<object[]>
			{
				new object[]
				{
					0, 0, new Point(0, 0)
				},
				new object[]
				{
					1, 1, new Point(1, 1)
				},
				new object[]
				{
					10, 11, new Point(10, 11)
				},
				new object[]
				{
					10.10, 11.11, new Point(10.10, 11.11)
				},
				new object[]
				{
					.10, .11, new Point(.10, .11)
				}
			};
		}
	}
}
