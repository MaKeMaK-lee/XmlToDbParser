using XmlToDbParser.Entities.Xml;

namespace XmlToDbParser.Comparers
{
    public class ParsedOrderComparerById : IEqualityComparer<ordersOrder>
    {
        public bool Equals(ordersOrder? x, ordersOrder? y)
        {
            return x?.no == y?.no;
        }

        public int GetHashCode(ordersOrder obj)
        {
            return Int32.Parse(obj.no);
        }
    }
}
