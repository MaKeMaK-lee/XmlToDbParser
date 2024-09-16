using System.Globalization;
using XmlToDbParser.Database;
using XmlToDbParser.Entities;
using XmlToDbParser.Entities.Xml;

namespace XmlToDbParser
{
    public static class OrdersParsingExtensions
    {
        public static bool IsEquivalentToMerge(this Product product, ordersOrderProduct xmlParsedInfo)
        {
            if (product.Article != xmlParsedInfo.name)
                return false;
            if (product.Price != double.Parse(xmlParsedInfo.price, CultureInfo.InvariantCulture))
                return false;

            return true;
        }

        public static bool IsEquivalentToMerge(this Client client, ordersOrderUser xmlParsedInfo)
        {
            if (client.ContactInfo!.Email != xmlParsedInfo.email)
                return false;
            if (client.ContactInfo!.Name != xmlParsedInfo.fio)
                return false;

            return true;
        }

        public static T AddGet<T>(this List<T> list, T item)
        {
            list.Add(item);
            return item;
        }

        public static List<Order> ToOrderListDatabaseBinded(this orders parsedOrders, XmlToDbParserDatabase database)
        {
            var list = new List<Order>(parsedOrders.Items.Length);
            var clients = new List<Client>();
            var products = new List<Product>();

            foreach (var parsedOrder in parsedOrders.Items)
            {
                list.Add
                (
                   (new Order()
                   {
                       Id = Int32.Parse(parsedOrder.no),
                       DateOfCreation = parsedOrder.reg_date,
                       OrderProducts = parsedOrder.product.Select
                        (
                            parsedOrderProduct => new OrderProduct()
                            {
                                OrderId = Int32.Parse(parsedOrder.no),
                                ProductCount = Int32.Parse(parsedOrderProduct.quantity),
                                Product = database.TryGetProduct(parsedOrderProduct.name, double.Parse(parsedOrderProduct.price, CultureInfo.InvariantCulture))
                                ?? products.FirstOrDefault(e => e.IsEquivalentToMerge(parsedOrderProduct),
                                products.AddGet(new()
                                {
                                    Article = parsedOrderProduct.name,
                                    Price = double.Parse(parsedOrderProduct.price, CultureInfo.InvariantCulture)
                                }))
                            }
                        ).ToList(),
                       Client = database.TryGetClient(parsedOrder.user.Single().email)
                       ?? clients.FirstOrDefault(e => e.IsEquivalentToMerge(parsedOrder.user.Single()),
                       clients.AddGet(new()
                       {
                           ContactInfo = new()
                           {
                               Email = parsedOrder.user.Single().email,
                               Name = parsedOrder.user.Single().fio
                           }
                       }))
                   }).InitializeMissingValues()
                );
            }

            return list;
        }
    }
}
