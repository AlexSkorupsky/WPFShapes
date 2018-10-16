using System;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

using Point = DrawShape.Entities.Point;

namespace DrawShape.Tools
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
        public static bool PointIsInBrokenLine(Point point, Polyline brokenLine)
		{
            var eps = 4.7;
            for (var i = 0; i<brokenLine.Points.Count; ++i)
            {
                var next = (i + 1) % brokenLine.Points.Count;
                if (distFromPointToLine(point, brokenLine.Points[i], brokenLine.Points[next]) < eps)
                {
                    return true;
                }
            }
            return false;
		}

        public static double distFromPointToPoint(Point a, Point b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double distFromPointToLine(Point x, System.Windows.Point startPoint, System.Windows.Point finishPoint)
        {
            Point a = new Point(startPoint.X, startPoint.Y);
            Point b = new Point(finishPoint.X, finishPoint.Y);
            double ret = distFromPointToPoint(x, a);
            double eps = 1e-4;
            double l = 0;
            double r = distFromPointToPoint(a, b);
            Point directionVector = new Point((b.X-a.X)/r, (b.Y - a.Y)/r);
            while (Math.Abs(r-l) > eps)
            {
                double m1 = l + (r - l) / 3;
                double m2 = r - (r - l) / 3;

                Point pointAtM1 = new Point(a.X + directionVector.X * m1, a.Y + directionVector.Y * m1);
                Point pointAtM2 = new Point(a.X + directionVector.X * m2, a.Y + directionVector.Y * m2);

                if (distFromPointToPoint(x, pointAtM1) < distFromPointToPoint(x, pointAtM2))
                {
                    ret = Math.Min(ret, distFromPointToPoint(x, pointAtM1));
                    r = m2;
                } else
                {
                    ret = Math.Min(ret, distFromPointToPoint(x, pointAtM2));
                    l = m1;
                }
            }
            return ret;
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
			var brokenLines = elements.OfType<Polyline>();
			var enumerable = brokenLines as Polyline[] ?? brokenLines.ToArray();
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
