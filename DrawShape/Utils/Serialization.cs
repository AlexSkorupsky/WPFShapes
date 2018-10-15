using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

using DrawShape.Classes;

namespace DrawShape.Utils
{
	/// <summary>
	/// Static class for hexagon serialization.
	/// </summary>
	public static class Serialization
	{
		/// <summary>
		/// Serializes a list of hexagons to a file.
		/// </summary>
		/// <param name="fileName">Path to target file.</param>
		/// <param name="hexagons">A list of hexagons to serialize.</param>
		public static void SerializeHexagons(string fileName, List<Hexagon> hexagons)
		{
			Stream stream = new FileStream(fileName, FileMode.Create);
			new XmlSerializer(typeof(List<Hexagon>)).Serialize(stream, hexagons);
			stream.Close();
		}

		/// <summary>
		/// Deserializes hexagons to a list.
		/// </summary>
		/// <param name="fileName">Path to source file.</param>
		/// <returns>A list of deserialized hexagons.</returns>
		public static List<Hexagon> DeserializeHexagons(string fileName)
		{
			Stream stream = new FileStream(fileName, FileMode.Open);
			var result = new XmlSerializer(typeof(List<Hexagon>)).Deserialize(stream) as List<Hexagon>;
			stream.Close();
			return result;
		}
	}
}
