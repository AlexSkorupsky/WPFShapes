using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;

using DrawShape.Utils;
using DrawShape.Classes;

using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace DrawShape.BL
{
	/// <summary>
	/// Class for business logick of the application.
	/// </summary>
	public static class FormBl
	{
		/// <summary>
		/// Function to pick a new colour.
		/// </summary>
		/// <param name="current">Parameter where the picked colour will be stored.</param>
		/// <param name="picker">Rectangle where picked colour will be displayed.</param>
		public static void SetColor(ref Brush current, ref Rectangle picker)
		{
			if (!GetColorFromColorDilog(out var color))
			{
				return;
			}

			current = color;
			picker.Fill = color;
		}

		/// <summary>
		/// Function to read hexagons from xml file.
		/// </summary>
		/// <returns>Hexagons read.</returns>
		public static IEnumerable<BrokenLine> ReadHexagons()
		{
			var ofd = new OpenFileDialog { Filter = @"Xml files (*.xml)|*.xml", DefaultExt = "xml", AddExtension = true };
			return ofd.ShowDialog() == true ? Serialization.DeserializeBrokenLines(ofd.FileName) : null;
		}

		/// <summary>
		/// Function to save hexagons to xml file.
		/// </summary>
		/// <param name="canvas">Canvas from which hexagons are to be saved.</param>
		public static void SaveHexagons(ref Canvas canvas)
		{
			var sfd = new SaveFileDialog { Filter = @"Xml files (*.xml)|*.xml", DefaultExt = "xml", FileName = "Rectangles", AddExtension = true };
			if (sfd.ShowDialog() != true)
			{
				return;
			}

			var hexagons = new List<BrokenLine>();
			foreach (var item in canvas.Children)
			{
				if (item is Polygon polygon)
				{
					hexagons.Add(BrokenLine.FromPolygon(polygon));
				}
			}

			Serialization.SerializeBrokenLines(sfd.FileName, hexagons);
		}
		
		/// <summary>
		/// Function to get colour with ColorDialog.
		/// </summary>
		/// <param name="brush">Variable to store picked colour.</param>
		/// <returns>True if colour was picked.</returns>
		public static bool GetColorFromColorDilog(out Brush brush)
		{
			var colorDialog = new ColorDialog { AllowFullOpen = true };
			var dialogResult = colorDialog.ShowDialog();
			brush = new SolidColorBrush();
			if (dialogResult != DialogResult.OK)
			{
				return false;
			}

			brush = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
			return true;
		}

		/// <summary>
		/// Function to display message box.
		/// </summary>
		/// <param name="msg">Message to be displayed.</param>
		public static void MessageBoxFatal(string msg)
		{
			MessageBox.Show(msg, "Fatal", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
