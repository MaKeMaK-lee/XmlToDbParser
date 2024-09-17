using XmlToDbParser.Database;

namespace XmlToDbParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using XmlToDbParserDatabase database = new("XmlToDbParserDatabase_1.db");

            var xmlOrders = XmlOrdersParser.Parse("example.xml");
            if (xmlOrders == null)
                return;

            xmlOrders.AddOrUpdateTo(database);


        }
    }
}
