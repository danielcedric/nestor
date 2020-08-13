using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Nestor.Tools.Helpers
{
    public class XmlHelper
    {
        private static Hashtable _hshTable;

        private static Hashtable hshTable
        {
            get
            {
                if (_hshTable == null) _hshTable = new Hashtable();
                return _hshTable;
            }
        }

        public static string GetFilename(object aObject)
        {
            return (string) hshTable[aObject];
        }


        public static void SerializeXMLObject(object ToSerialize, XmlSerializerNamespaces ns, string aFilename)
        {
            #region vérification d'usage de l'existence du fichier (sinon si le dossier n'existe pas la serialization plante)

            var targetFileInfo = new FileInfo(aFilename);
            if (!targetFileInfo.Directory.Exists) targetFileInfo.Directory.Create();

            #endregion

            var Serialiser = new XmlSerializer(ToSerialize.GetType());
            TextWriter Writer = new StreamWriter(aFilename);

            try
            {
                Serialiser.Serialize(Writer, ToSerialize, ns);
            }
            finally
            {
                Writer.Close();
            }
        }

        public static void SerializeXMLObject(object ToSerialize, XmlSerializerNamespaces ns)
        {
            SerializeXMLObject(ToSerialize, ns, GetFilename(ToSerialize));
        }

        public static T DeserializeXMLObject<T>(string aFilename) where T : class, new()
        {
            var aTObject = new T();
            DeserializeXMLObject(aFilename, out aTObject);
            return aTObject;
        }

        public static T DeserializeXMLObject<T>(Stream ms) where T : class, new()
        {
            var aTObject = new T();
            DeserializeXMLObject(ms, out aTObject);
            return aTObject;
        }

        public static XmlNode SerializeObjectToXmlNode<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Argument cannot be null");

            XmlNode resultNode = null;
            var xmlSerializer = new XmlSerializer(obj.GetType());
            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    xmlSerializer.Serialize(memoryStream, obj);
                }
                catch (InvalidOperationException)
                {
                    return null;
                }

                memoryStream.Position = 0;
                var doc = new XmlDocument();
                doc.Load(memoryStream);
                resultNode = doc.DocumentElement;
            }

            return resultNode;
        }


        public static T DeSerializeXmlNodeToObject<T>(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException("Argument cannot be null");
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var memoryStream = new MemoryStream())
            {
                var doc = new XmlDocument();
                doc.AppendChild(doc.ImportNode(node, true));
                doc.Save(memoryStream);
                memoryStream.Position = 0;
                var reader = XmlReader.Create(memoryStream);
                try
                {
                    return (T) xmlSerializer.Deserialize(reader);
                }
                catch
                {
                    return typeof(T).IsByRef ? default : (T) Activator.CreateInstance(typeof(T));
                }
            }
        }

        /// <summary>
        ///     Déserialise un flux XML contenu dans un stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ms"></param>
        /// <param name="aTObject"></param>
        public static void DeserializeXMLObject<T>(Stream ms, out T aTObject) where T : class
        {
            ms.Seek(0, SeekOrigin.Begin);

            var Serializer = new XmlSerializer(typeof(T));
            XmlReader Reader = new XmlTextReader(ms);

            try
            {
                var temp = Serializer.Deserialize(Reader);
                aTObject = (T) temp;
            }
            finally
            {
                Reader.Close();
            }
        }

        public static void DeserializeXMLObject<T>(string aFilename, out T aTObject) where T : class
        {
            var Serializer = new XmlSerializer(typeof(T));
            XmlReader Reader = new XmlTextReader(aFilename);

            try
            {
                var temp = Serializer.Deserialize(Reader);
                aTObject = (T) temp;

                if (!hshTable.ContainsKey(aTObject))
                    hshTable.Add(aTObject, aFilename);
                else
                    hshTable[aTObject] = aFilename;
            }
            finally
            {
                Reader.Close();
            }
        }

        /// <summary>
        ///     Création d'un noeud XML
        /// </summary>
        /// <param name="xmlDoc">document XML</param>
        /// <param name="nodeName">nom du noeud</param>
        /// <param name="nodeValue">valeur du noeud (CDATA)</param>
        /// <param name="writeCDataSection">true si on souhaite écrire la valeur CDATA, false sinon</param>
        /// <returns>noeud XML</returns>
        public static XmlNode Create(XmlDocument xmlDoc, string nodeName, string nodeValue, bool writeCDataSection)
        {
            XmlNode node = xmlDoc.CreateElement(nodeName);
            if (writeCDataSection)
                node.AppendChild(xmlDoc.CreateCDataSection(nodeValue));
            else
                node.AppendChild(xmlDoc.CreateTextNode(nodeValue));

            return node;
        }

        /// <summary>
        ///     Création d'un noeud XML
        /// </summary>
        /// <param name="xmlDoc">document XML</param>
        /// <param name="nodeName">nom du noeud</param>
        /// <param name="nodeValue">valeur du noeud (CDATA)</param>
        /// <param name="writeCDataSection">true si on souhaite écrire la valeur CDATA, false sinon</param>
        /// <returns>noeud XML</returns>
        public static XmlNode Create(XmlDocument xmlDoc, string nodeName, string nodeValue)
        {
            return Create(xmlDoc, nodeName, nodeValue, true);
        }

        /// <summary>
        ///     Création d'un noeud XML avec un attribut
        /// </summary>
        /// <param name="xmlDoc">document XML</param>
        /// <param name="nodeName">nom du noeud</param>
        /// <param name="nodeValue">valeur du noeud (CDATA)</param>
        /// <param name="attributeName">nom du champ</param>
        /// <param name="attributeValue">valeur du champ</param>
        /// <returns>noeud XML</returns>
        public static XmlNode Create(XmlDocument xmlDoc, string nodeName, string nodeValue, string attributeName,
            string attributeValue)
        {
            var node = Create(xmlDoc, nodeName, nodeValue);

            var attribute = xmlDoc.CreateAttribute(attributeName);
            attribute.Value = attributeValue;
            node.Attributes.Append(attribute);

            return node;
        }

        /// <summary>
        ///     Création d'un noeud XML sans valeur mais avec un attribut
        /// </summary>
        /// <param name="xmlDoc">document XML</param>
        /// <param name="nodeName">nom du noeud</param>
        /// <param name="nodeValue">valeur du noeud (CDATA)</param>
        /// <param name="attributeName">nom du champ</param>
        /// <param name="attributeValue">valeur du champ</param>
        /// <returns>noeud XML</returns>
        public static XmlNode Create(XmlDocument xmlDoc, string nodeName, string attributeName, string attributeValue)
        {
            XmlNode node = xmlDoc.CreateElement(nodeName);

            var attribute = xmlDoc.CreateAttribute(attributeName);
            attribute.Value = attributeValue;
            node.Attributes.Append(attribute);

            return node;
        }
    }
}