using System.Globalization;
using XmlToDbParser.Database;
using XmlToDbParser.Entities;
using XmlToDbParser.Entities.Xml;

namespace XmlToDbParser
{
    public static class OrdersParsingExtensions
    {
        private static bool IsEquivalentToMerge(this Product product, ordersOrderProduct xmlParsedInfo)
        {
            if (product.Article != xmlParsedInfo.name)
                return false;
            if (product.Price != double.Parse(xmlParsedInfo.price, CultureInfo.InvariantCulture))
                return false;

            return true;
        }

        private static bool IsEquivalentToMerge(this Client client, ordersOrderUser xmlParsedInfo)
        {
            if (client.ContactInfo!.Email != xmlParsedInfo.email)
                return false;
            if (client.ContactInfo!.Name != xmlParsedInfo.fio)
                return false;

            return true;
        }

        private static bool IsEquivalentToMerge(this ICollection<OrderProduct> orderProducts, ordersOrderProduct[] xmlParsedInfo)
        {
            if (orderProducts.All(orderProduct =>
                xmlParsedInfo.Any(parsedOrderProduct =>
                    orderProduct.Product.IsEquivalentToMerge(parsedOrderProduct)
                    && orderProduct.ProductCount == Int32.Parse(parsedOrderProduct.quantity))))
                return true;
            return false;
        }

        private static T AddGet<T>(this List<T> list, T item)
        {
            list.Add(item);
            return item;
        }

        private static Product CreateProduct(ordersOrderProduct parsedOrderProduct)
        {
            return new()
            {
                Article = parsedOrderProduct.name,
                Price = double.Parse(parsedOrderProduct.price, CultureInfo.InvariantCulture)
            };
        }

        private static Client CreateClient(ordersOrderUser parsedUser)
        {
            return new()
            {
                ContactInfo = new()
                {
                    Email = parsedUser.email,
                    Name = parsedUser.fio
                }
            };
        }

        public static void AddOrUpdateTo(this orders parsedOrders, XmlToDbParserDatabase database)
        {
            var ordersToUpdate = database.GetOrders(parsedOrders.Items.Select(parsedOrder => Int32.Parse(parsedOrder.no)));

            var list = new List<Order>(parsedOrders.Items.Length);
            var clients = new List<Client>();
            var products = new List<Product>();

            foreach (var parsedOrder in parsedOrders.Items)
            {
                var existingOrder = ordersToUpdate.FirstOrDefault(order => order.Id == Int32.Parse(parsedOrder.no));
                if (existingOrder != null)
                {
                    if (existingOrder.DateOfCreation != parsedOrder.reg_date)
                    {
                        existingOrder.DateOfCreation = parsedOrder.reg_date;
                    }
                    if (!existingOrder.OrderProducts.IsEquivalentToMerge(parsedOrder.product))
                    {
                        existingOrder.OrderProducts = parsedOrder.product.Select
                        (
                            parsedOrderProduct => new OrderProduct()
                            {
                                OrderId = Int32.Parse(parsedOrder.no),
                                ProductCount = Int32.Parse(parsedOrderProduct.quantity),
                                Product = database.TryGetProduct(parsedOrderProduct.name, double.Parse(parsedOrderProduct.price, CultureInfo.InvariantCulture))
                                    ?? products.FirstOrDefault(e => e.IsEquivalentToMerge(parsedOrderProduct),
                                    products.AddGet(CreateProduct(parsedOrderProduct)))
                            }
                        ).ToList();
                    }
                    if (!existingOrder.Client.IsEquivalentToMerge(parsedOrder.user.Single()))
                    {
                        existingOrder.Client = database.TryGetClient(parsedOrder.user.Single().email)
                            ?? clients.FirstOrDefault(e => e.IsEquivalentToMerge(parsedOrder.user.Single()),
                            clients.AddGet(CreateClient(parsedOrder.user.Single())));
                    }
                }
                else
                {
                    list.Add(new Order()
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
                                    products.AddGet(CreateProduct(parsedOrderProduct)))
                            }
                        ).ToList(),
                        Client = database.TryGetClient(parsedOrder.user.Single().email)
                            ?? clients.FirstOrDefault(e => e.IsEquivalentToMerge(parsedOrder.user.Single()),
                            clients.AddGet(CreateClient(parsedOrder.user.Single())))
                    }.InitializeMissingValues());
                }
            }

            database.Add(list);
            database.Save();
        }
    }
}
