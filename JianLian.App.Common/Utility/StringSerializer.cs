using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Gym.App.Common
{
    public class StringSerializer
    {

        public static string Serialize<T>(T model) where T : class
        {
            if (null != model)
            {
                try
                {
                    var data = new XmlWrapper<T>();
                    data.Data = model;
                    string xml = "";
                    XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                    xmlSerializerNamespaces.Add("", "");
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                    xmlWriterSettings.OmitXmlDeclaration = true;
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlWrapper<T>));
                    using (MemoryStream xmlStream = new MemoryStream())
                    {
                        using (XmlWriter xmlWriter = XmlWriter.Create(xmlStream, xmlWriterSettings))
                        {
                            xmlSerializer.Serialize(xmlWriter, data, xmlSerializerNamespaces);
                        }
                        xmlStream.Position = 0;
                        StreamReader sr = new StreamReader(xmlStream);
                        xml = sr.ReadToEnd();
                        sr.Dispose();
                    }
                    return xml;
                }
                catch
                {
                    return string.Empty;
                }
            }
            else
                return string.Empty;
        }

        public static T DeSerialize<T>(string xml) where T : class, new()
        {
            try
            {
                var model = new XmlWrapper<T>();
                using (StringReader stringReader = new StringReader(xml))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlWrapper<T>));
                    model = (XmlWrapper<T>)xmlSerializer.Deserialize(stringReader);
                }
                if (null != model && null != model.Data)
                    return model.Data;
                else
                    return new T();
            }
            catch
            {
                return new T();
            }
        }
        /// <summary>
        /// 模糊搜索前缀
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AddLikeSearchPrefix(string value)
        {
            return "%" + value + "%";
        }

    }

    [XmlRoot("xml")]
    public class XmlWrapper<T> where T : class
    {
        [XmlElement("data")]
        public T Data
        {
            get; set;
        }
    }
}
