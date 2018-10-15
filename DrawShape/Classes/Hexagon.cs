using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace DrawShape.Classes
{
	/// <summary>
	/// Class to represent a hexagon in two-dimensional space.
	/// </summary>
	[Serializable]
	public class Hexagon
	{
		/// <summary>
		/// Class to represent a border of hexagon.
		/// </summary>
		public class BorderColor
		{
			/// <summary>
			/// Gets/Sets an amount of red colour in RGB specification.
			/// </summary>
			[XmlAttribute]
			public int R { get; set; }

			/// <summary>
			/// Gets/Sets an amount of green colour in RGB specification.
			/// </summary>
			[XmlAttribute]
			public int G { get; set; }

			/// <summary>
			/// Gets/Sets an amount of blue colour in RGB specification.
			/// </summary>
			[XmlAttribute]
			public int B { get; set; }

			/// <summary>
			/// Function to set colour of the border.
			/// </summary>
			public BorderColor()
			{
			}

			/// <summary>
			/// Function to set colour of the border using rgb specification.
			/// </summary>
			/// <param name="r">Amount of red.</param>
			/// <param name="g">Amount of green.</param>
			/// <param name="b">Amount of blue.</param>
			public BorderColor(int r, int g, int b)
			{
				R = r;
				G = g;
				B = b;
			}
		}
		
		/// <summary>
		/// Gets/Sets a name of hexagon.
		/// </summary>
		[XmlAttribute]
		public string Name { get; set; }

		/// <summary>
		/// Gets/Sets a colour of the hexagon border.
		/// </summary>
		[XmlElement]
		public BorderColor ColorBorder { get; set; }

		/// <summary>
		/// Gets/Sets an array of points of hexagon.
		/// </summary>
		[XmlArray]
		public Point[] Points { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Hexagon"/> class.  
		/// </summary>
		public Hexagon()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Hexagon"/> class. 
		/// </summary>
		/// <param name="name">Name of hexagon.</param>
		/// <param name="points">Points of hexagon vertices.</param>
		/// <param name="fillBrush">Colour of hexagon.</param>
		/// <param name="borderBrush">Colour of hexagon border.</param>
		public Hexagon(string name, List<Point> points, Brush borderBrush)
		{
			if (points == null)
			{
				throw new NullReferenceException("points are null");
			}
			
			if (points.Count != 6)
			{
				throw new InvalidDataException("hexagon requires six points");
			}
			
			Name = name;
			Points = points.ToArray();
			var colorBorder = ((SolidColorBrush)borderBrush).Color;
			ColorBorder = new BorderColor(colorBorder.R, colorBorder.G, colorBorder.B);
		}

		/// <summary>
		/// Function to convert hexagon type to polygon type.
		/// </summary>
		/// <returns>Hexagon shape of type <see cref="Polygon"/></returns>
		public Polygon ToPolygon()
		{
			if (Points == null)
			{
				throw new NullReferenceException("points are null");
			}
			
			if (Points.Length != 6)
			{
				throw new InvalidDataException("hexagon requires six points");
			}

			var polygon = new Polygon();
			foreach (var point in Points)
			{
				polygon.Points.Add(new System.Windows.Point(point.X, point.Y));
			}
			
			polygon.Stroke = new SolidColorBrush(Color.FromRgb((byte)ColorBorder.R, (byte)ColorBorder.G, (byte)ColorBorder.B));
			polygon.StrokeThickness = 2;
			
			polygon.Name = Name;
			return polygon;
		}

		/// <summary>
		/// Function to convert polygon type to hexagon type.
		/// </summary>
		/// <returns>Hexagon shape of type <see cref="Hexagon"/></returns>
		public static Hexagon FromPolygon(Polygon polygon)
		{
			var points = new List<Point>();
			foreach (var point in polygon.Points)
			{
				points.Add(new Point(point.X, point.Y));
			}

			return new Hexagon(polygon.Name, points, polygon.Stroke);
		}
	}
}
