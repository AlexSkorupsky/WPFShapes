using System;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

using Point = DrawShape.Classes.Point;

namespace DrawShape.Utils
{
	/// <summary>
	/// Utility class.
	/// </summary>
	public static class Util
	{
        /// <summary>
        /// Function to check if point is in BrokenLine.
        /// </summary>
        /// <param name="point">Point to check.</param>
        /// <param name="brokenLine">BrokenLine in which point might be.</param>
        /// <returns>True if point is located in given BrokenLine.</returns>
        public static bool PointIsInBrokenLine(Point point, Polygon brokenLine)
		{
			// Create a point for line segment from p to infinite 
			var extreme = new System.Windows.Point(10000, point.Y);

			// Count intersections of the above line with sides of polygon 
			int count = 0, i = 0;
			do
			{
				var next = (i + 1) % 6;

				// Check if the line segment from 'p' to 'extreme' intersects 
				// with the line segment from 'polygon[i]' to 'polygon[next]' 
				if (AreSidesIntersected(brokenLine.Points[i], brokenLine.Points[next], new System.Windows.Point(point.X, point.Y),  extreme))
				{
					// If the point 'p' is colinear with line segment 'i-next', 
					// then check if it lies on segment. If it lies, return true, 
					// otherwise false 
					if (Orientation(brokenLine.Points[i], new System.Windows.Point(point.X, point.Y), brokenLine.Points[next]) == 0)
					{
						return OnSegment(brokenLine.Points[i], new System.Windows.Point(point.X, point.Y), brokenLine.Points[next]);
					}

					count++;
				}

				i = next;
			}
			while (i != 0);

			// Return true if count is odd, false otherwise 
			return count % 2 == 1;
		}

		public static bool OnSegment(System.Windows.Point p, System.Windows.Point q, System.Windows.Point r)
		{
			return q.X <= Math.Max(p.X, r.X)
				   && q.X >= Math.Min(p.X, r.X)
				   && q.Y <= Math.Max(p.Y, r.Y)
				   && q.Y >= Math.Min(p.Y, r.Y);
		}

		/// <summary>
		/// To find orientation of ordered triplet (p, q, r). 
		/// The function returns following values 
		/// 0 --> p, q and r are collinear 
		/// 1 --> Clockwise 
		/// 2 --> Counterclockwise 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="q"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public static int Orientation(System.Windows.Point p, System.Windows.Point q, System.Windows.Point r)
		{
			var val = ((q.Y - p.Y) * (r.X - q.X)) - ((q.X - p.X) * (r.Y - q.Y));
			if (val.Equals(0))
			{
				return 0; // collinear 
			}

			return (val > 0) ? 1 : 2; // clock or counterclockwise
		}

		public static bool AreSidesIntersected(
			System.Windows.Point firstSidePointOne,
			System.Windows.Point firstSidePointTwo,
			System.Windows.Point secondSidePointOne,
			System.Windows.Point secondSidePointTwo)
		{
			var o1 = Orientation(firstSidePointOne, firstSidePointTwo, secondSidePointOne);
			var o2 = Orientation(firstSidePointOne, firstSidePointTwo, secondSidePointTwo);
			var o3 = Orientation(secondSidePointOne, secondSidePointTwo, firstSidePointOne);
			var o4 = Orientation(secondSidePointOne, secondSidePointTwo, firstSidePointTwo);

			// General case 
			if (o1 != o2 && o3 != o4)
			{
				return true;
			}

			// Special Cases 
			// p1, q1 and p2 are collinear and p2 lies on segment p1q1 
			if (o1 == 0 && OnSegment(firstSidePointOne, secondSidePointOne, firstSidePointTwo))
			{
				return true;
			}

			// p1, q1 and p2 are collinear and q2 lies on segment p1q1 
			if (o2 == 0 && OnSegment(firstSidePointOne, secondSidePointTwo, firstSidePointTwo))
			{
				return true;
			}

			// p2, q2 and p1 are collinear and p1 lies on segment p2q2 
			if (o3 == 0 && OnSegment(secondSidePointOne, firstSidePointOne, secondSidePointTwo))
			{
				return true;
			}

			// p2, q2 and q1 are collinear and q1 lies on segment p2q2 
			if (o4 == 0 && OnSegment(secondSidePointOne, firstSidePointTwo, secondSidePointTwo))
			{
				return true;
			}

			return false;
		}

        /// <summary>
        /// Returns an index of a BrokenLine with specified name.
        /// </summary>
        /// <param name="name">BrokenLine's name.</param>
        /// <param name="elements">A list of available BrokenLines.</param>
        /// <returns>Index of BrokenLine.</returns>
        /// <exception cref="InvalidDataException">Throws if BrokenLine does not exist.</exception>
        public static int GetBrokenLineIdByName(string name, UIElementCollection elements)
		{
			var brokenLines = elements.OfType<Polygon>();
			var enumerable = brokenLines as Polygon[] ?? brokenLines.ToArray();
			for (var i = 0; i < enumerable.Length; i++)
			{
				if (enumerable[i].Name == name)
				{
					return i;
				}
			}

			throw new InvalidDataException("Broken line does not exist");
		}
		
		/// <summary>
		/// Function to get line from given coordinates.
		/// </summary>
		/// <param name="start">Starting point.</param>
		/// <param name="end">Ending point.</param>
		/// <param name="brush">Colour of the line.</param>
		/// <returns></returns>
		public static Line GetLine(Point start, Point end, Brush brush)
		{
			var line = new Line { X1 = start.X, Y1 = start.Y, X2 = end.X, Y2 = end.Y, StrokeThickness = 1, Stroke = brush, SnapsToDevicePixels = true };
			line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
			return line;
		}
	}
}
