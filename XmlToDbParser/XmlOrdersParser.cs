using System.Xml.Serialization;
using XmlToDbParser.Entities.Xml;

namespace XmlToDbParser
{
    public static class XmlOrdersParser
    {
        public static orders? Parse(string filePath)
        {
            ArgumentNullException.ThrowIfNull(filePath);

            XmlSerializer xmlSerializer = new(typeof(orders));
            using FileStream fs = new(filePath, FileMode.Open);
            return xmlSerializer.Deserialize(fs) as orders;
        }
    }
}
