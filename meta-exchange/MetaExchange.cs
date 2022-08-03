using meta_exchange.models;
using Newtonsoft.Json;

namespace meta_exchange
{
    public class MetaExchange
    {
        private const string APP_TAG = "[meta-exchange]";

        public Dictionary<string, OrderBook> GetOrderbooks(string dataPath)
        {
            try
            {
                if (File.Exists(dataPath) && dataPath.EndsWith(".json"))
                {
                    string[] data = File.ReadAllLines(dataPath);
                    Dictionary<string, OrderBook> orderBooks = new();
                    foreach (string dataEntry in data)
                    {
                        string id = dataEntry.Split('\t')[0];
                        string value = dataEntry.Split('\t')[1];
                        OrderBook orderBookEntry = JsonConvert.DeserializeObject<OrderBook>(value);
                        orderBooks.Add(id, orderBookEntry);
                    }
                    return orderBooks;
                }
                throw new FileNotFoundException(string.Format("The file {0} was not found or isn't a json file.", dataPath));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("{0} ERROR: {1}", APP_TAG, e.Message));
                return new();
            }
        }

        public List<Order> CreateOrders(OrderBook orderBook, decimal eurBalance, decimal btcBalance, string orderType, decimal orderAmount)
        {
            try
            {
                Console.WriteLine(string.Format("{0} Trying to {1} {2} BTC...", APP_TAG, orderType, orderAmount.ToString()));

                List<Order> orders = new();
                if (orderType.Equals("Buy"))
                {
                    if (eurBalance <= 0) throw new ArgumentException("EUR balance too low for buying.");

                    List<Bid> bids = orderBook.Bids;
                    bids = bids.OrderBy(bid => bid.Order.Price).ToList();
                    foreach (Bid bid in bids)
                    {
                        decimal orderPrice = Convert.ToDecimal(bid.Order.Price);
                        decimal orderSize = Convert.ToDecimal(bid.Order.Amount);

                        if (orderSize >= orderAmount)
                        {
                            orderSize = orderAmount;
                        }
                        if ((orderSize * orderPrice) >= eurBalance)
                        {
                            orderSize = eurBalance / orderPrice;
                        }

                        Order order = new()
                        {
                            Id = null,
                            Time = DateTime.Now,
                            Type = orderType,
                            Kind = "Limit",
                            Price = Convert.ToDouble(orderPrice),
                            Amount = Convert.ToDouble(orderSize)
                        };
                        orders.Add(order);

                        eurBalance -= orderSize * orderPrice;
                        btcBalance += orderSize;
                        orderAmount -= orderSize;
                        Console.WriteLine(string.Format("{0} created BUY order for {1} BTC at {2} EUR - cost: {3} EUR - new BTC balance: {4} BTC - new EUR balance: {5} EUR", APP_TAG, orderSize.ToString(), orderPrice.ToString(), (orderSize * orderPrice).ToString(), btcBalance.ToString(), eurBalance.ToString()));

                        if (orderAmount <= 0 || eurBalance <= 0) break;
                    }
                }
                else if (orderType.Equals("Sell"))
                {
                    if (btcBalance <= 0) throw new ArgumentException("BTC balance too low for selling.");

                    List<Ask> asks = orderBook.Asks;
                    asks = asks.OrderByDescending(x => x.Order.Price).ToList();
                    foreach (Ask ask in asks)
                    {
                        decimal orderPrice = Convert.ToDecimal(ask.Order.Price);
                        decimal orderSize = Convert.ToDecimal(ask.Order.Amount);

                        if (orderSize >= orderAmount)
                        {
                            orderSize = orderAmount;
                        }
                        if (orderAmount >= btcBalance)
                        {
                            orderSize = btcBalance;
                        }

                        Order order = new()
                        {
                            Id = null,
                            Time = DateTime.Now,
                            Type = orderType,
                            Kind = "Limit",
                            Price = Convert.ToDouble(orderPrice),
                            Amount = Convert.ToDouble(orderSize)
                        };
                        orders.Add(order);

                        btcBalance -= orderSize;
                        eurBalance += orderSize * orderPrice;
                        orderAmount -= orderSize;
                        Console.WriteLine(string.Format("{0} created SELL order for {1} BTC at {2} EUR - sold for: {3} EUR - EUR balance: {4} EUR - BTC balance: {5} BTC", APP_TAG, orderSize.ToString(), orderPrice.ToString(), (orderSize * orderPrice).ToString(), eurBalance.ToString(), btcBalance.ToString()));

                        if (orderAmount <= 0 || btcBalance <= 0) break;
                    }
                }
                else
                {
                    throw new NotSupportedException(string.Format("The order type {0} is not supported (\"Buy\" / \"Sell\")", orderType));
                }
                return orders;
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("{0} ERROR: {1}", APP_TAG, e.Message));
                return new();
            }
        }
    }
}