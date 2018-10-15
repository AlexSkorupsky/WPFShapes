using System;
using System.Xml.Serialization;

namespace DrawShape.Classes
{
	/// <summary>
	/// Class to represent a point in two-dimensional space.
	/// </summary>
	[Serializable]
	public class Point
	{
		/// <summary>
		/// X coordinate of a point.
		/// </summary>
		[XmlAttribute]
		public double X { get; set; }
		
		/// <summary>
		/// Y coordinate of a point.
		/// </summary>
		[XmlAttribute]
		public double Y { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Point"/> class.  
		/// </summary>
		public Point()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Point"/> class.  
		/// </summary>
		/// <param name="x">X coordinate of a point.</param>
		/// <param name="y">Y coordinate of a point.</param>
		public Point(double x, double y)
		{
			X = x;
			Y = y;
		}
	}
}
