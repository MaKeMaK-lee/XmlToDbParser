using System.Xml.Serialization;
using XmlToDbParser.Database;

namespace XmlToDbParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using XmlToDbParserDatabase database = new("XmlToDbParserDatabase_1.db");

            XmlSerializer xmlSerializer = new(typeof(Entities.Xml.orders));

            using FileStream fs = new("example.xml", FileMode.OpenOrCreate);
            Entities.Xml.orders? xmlOrders = xmlSerializer.Deserialize(fs) as Entities.Xml.orders;
            if (xmlOrders == null)
                return;

            var list = xmlOrders.ToOrderListDatabaseBinded(database);
            database.Add(list);
            database.Save();


        }
    }
}
