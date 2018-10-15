using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

using DrawShape.Classes;

namespace DrawShape.Utils
{
    /// <summary>
    /// Static class for BrokenLine serialization.
    /// </summary>
    public static class Serialization
	{
        /// <summary>
        /// Serializes a list of BrokenLines to a file.
        /// </summary>
        /// <param name="fileName">Path to target file.</param>
        /// <param name="brokenLines">A list of BrokenLines to serialize.</param>
        public static void SerializeBrokenLines(string fileName, List<BrokenLine> brokenLines)
		{
			Stream stream = new FileStream(fileName, FileMode.Create);
			new XmlSerializer(typeof(List<BrokenLine>)).Serialize(stream, brokenLines);
			stream.Close();
		}

        /// <summary>
        /// Deserializes BrokenLines to a list.
        /// </summary>
        /// <param name="fileName">Path to source file.</param>
        /// <returns>A list of deserialized BrokenLines.</returns>
        public static List<BrokenLine> DeserializeBrokenLines(string fileName)
		{
			Stream stream = new FileStream(fileName, FileMode.Open);
			var result = new XmlSerializer(typeof(List<BrokenLine>)).Deserialize(stream) as List<BrokenLine>;
			stream.Close();
			return result;
		}
	}
}
