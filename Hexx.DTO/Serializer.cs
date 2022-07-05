using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Hexx.DTO
{
    public class Serializer
    {
        private Serializer()
        {

        }

        public static string Serialize(object objectToSerialize, Type objectType)
        {
            XmlSerializer serializer = new XmlSerializer(objectType);
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    try
                    {
                        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                        namespaces.Add("", "");
                        serializer.Serialize(xmlWriter, objectToSerialize, namespaces);
                        string serializedXml = textWriter.ToString();
                        return serializedXml;
                    }
                    catch (Exception)
                    {
                        return string.Empty;
                    }
                }
            }
        }
    }
}