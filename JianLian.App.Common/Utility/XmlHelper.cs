using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gym.App.Common
{
    public class XmlHelper
    {
        //private static string XmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ThumbPath"]);

        public static T FileToObject<T>(string fullName) where T : new()
        {
            if (File.Exists(fullName))
            {
                using (Stream fStream = new FileStream(fullName, FileMode.Open, FileAccess.ReadWrite))
                {
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
                    return (T)xmlFormat.Deserialize(fStream);
                }
            }
            else
            {
                return default(T);
            }

        }

        public static void ObjectToFile<T>(T obj, string fullName) where T : new()
        {
            string fullPath = Path.GetDirectoryName(fullName);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            using (Stream fStream = new FileStream(fullName, FileMode.Create, FileAccess.ReadWrite))
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
                xmlFormat.Serialize(fStream, obj);
            }
        }

        public static string ObjectToString<T>(T obj) where T : new()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            Stream stream = new MemoryStream();
            xmlSerializer.Serialize(stream, obj);
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static T StringToObject<T>(string content) where T : new()
        {
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content)))
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
                return (T)xmlFormat.Deserialize(stream);
            }
        }
    }
}
