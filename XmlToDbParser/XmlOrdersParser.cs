using System.Xml.Serialization;

namespace XmlToDbParser
{
    public static class XmlOrdersParser
    {
        public static Entities.Xml.orders? Parse(string filePath)
        {
            XmlSerializer xmlSerializer = new(typeof(Entities.Xml.orders));
            using FileStream fs = new(filePath, FileMode.OpenOrCreate);
            return xmlSerializer.Deserialize(fs) as Entities.Xml.orders;
        }
    }
}
