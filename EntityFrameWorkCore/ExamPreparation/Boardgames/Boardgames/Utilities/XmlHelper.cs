﻿using System.Text;
using System.Xml.Serialization;

namespace Boardgames.Utilities
{
    public class XmlHelper
    {
        public T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);
            object? deserializedObjects = xmlSerializer.Deserialize(stringReader);

            if (deserializedObjects == null || deserializedObjects is not T deserializedObjectTypes)
            {
                throw new InvalidOperationException();
            }
            return deserializedObjectTypes;
        }

        public string Serialize<T>(T obj, string rootName)
        {
            StringBuilder sb = new StringBuilder();
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerialize = new XmlSerializer(typeof(T), xmlRoot);

            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);

            using StringWriter stringWriter = new StringWriter(sb);

            xmlSerialize.Serialize(stringWriter, obj, xmlNamespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
