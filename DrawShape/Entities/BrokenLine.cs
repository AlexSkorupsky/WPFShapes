using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace DrawShape.Entities
{
    /// <summary>
    /// Class to represent a broken line in two-dimensional space.
    /// </summary>
    [Serializable]
	public class BrokenLine
	{
        /// <summary>
        /// Class to represent a border of broken line.
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
        /// Gets/Sets a name of broken line.
        /// </summary>
        [XmlAttribute]
		public string Name { get; set; }

        /// <summary>
        /// Gets/Sets a colour of the broken line border.
        /// </summary>
        [XmlElement]
		public BorderColor ColorBorder { get; set; }

        /// <summary>
        /// Gets/Sets an array of points of broken line.
        /// </summary>
        [XmlArray]
		public Point[] Points { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BrokenLine"/> class.  
		/// </summary>
		public BrokenLine()
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="BrokenLine"/> class. 
        /// </summary>
        /// <param name="name">Name of broken line.</param>
        /// <param name="points">Points of broken line vertices.</param>
        /// <param name="fillBrush">Colour of broken line.</param>
        /// <param name="borderBrush">Colour of broken line border.</param>
        public BrokenLine(string name, List<Point> points, Brush borderBrush)
		{
			if (points == null)
			{
				throw new NullReferenceException("points are null");
			}
			
			Name = name;
			Points = points.ToArray();
			var colorBorder = ((SolidColorBrush)borderBrush).Color;
			ColorBorder = new BorderColor(colorBorder.R, colorBorder.G, colorBorder.B);
		}

        /// <summary>
        /// Function to convert broken line type to polyline type.
        /// </summary>
        /// <returns>broken line shape of type <see cref="Polyline"/></returns>
        public Polyline ToPolyline()
		{
			if (Points == null)
			{
				throw new NullReferenceException("points are null");
			}
			
			var Polyline = new Polyline();
			foreach (var point in Points)
			{
				Polyline.Points.Add(new System.Windows.Point(point.X, point.Y));
			}
			Polyline.Stroke = new SolidColorBrush(Color.FromRgb((byte)ColorBorder.R, (byte)ColorBorder.G, (byte)ColorBorder.B));
			Polyline.StrokeThickness = 2;
			Polyline.Name = Name;
			return Polyline;
		}

        /// <summary>
        /// Function to convert polyline type to broken line type.
        /// </summary>
        /// <returns>broken line shape of type <see cref="BrokenLine"/></returns>
        public static BrokenLine FromPolyline(Polyline polyline)
		{
			var points = new List<Point>();
			foreach (var point in polyline.Points)
			{
				points.Add(new Point(point.X, point.Y));
			}
			return new BrokenLine(polyline.Name, points, polyline.Stroke);
		}
	}
}
