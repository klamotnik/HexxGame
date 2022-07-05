using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Hexx.DTO
{
    public class Deserializer
    {
        private Deserializer()
        {

        }

        public static object Deserialize(string xml, Type objectType)
        {
            XmlSerializer XmlSerializer = new XmlSerializer(objectType);

            MemoryStream MemoryStream = new MemoryStream(StringToUTF8ByteArray(xml));
            return XmlSerializer.Deserialize(MemoryStream);
        }

        public static object Deserialize(XmlDocument xml, Type objectType)
        {
            return Deserialize(xml.OuterXml, objectType);
        }

        private static byte[] StringToUTF8ByteArray(string xml)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(xml);
        }
    }
}
