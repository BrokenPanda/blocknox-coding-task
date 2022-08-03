using meta_exchange;
using meta_exchange.models;
using Newtonsoft.Json;

namespace meta_exchange_cli
{
    internal class Program
    {
        private const string APP_TAG = "[meta-exchange-cli]";

        static void Main(string[] args)
        {
            try
            {
                if (args.Length.Equals(0) || args[0].Equals("--help") || args[0].Equals("-h") || !args.Length.Equals(5))
                {
                    Console.WriteLine("usage: meta-exchange-cli.exe <orderbooks-data-path> <eur-balance> <btc-balance> <order-type> <order-amount>\n");
                    Console.WriteLine("arguments:");
                    Console.WriteLine("\torderbook-data-path\tthe path to a JSON file containing orderbooks data");
                    Console.WriteLine("\teur-balance\t\tthe available EUR balance on a given cryptoexchange");
                    Console.WriteLine("\tbtc-balance\t\tthe available BTC balance on a given cryptoexchange");
                    Console.WriteLine("\torder-type\t\teither \"Buy\" or \"Sell\"");
                    Console.WriteLine("\torder-amount\t\torder size in BTC");
                    Environment.Exit(0);
                }

                Console.WriteLine(string.Format("{0} Starting service...", APP_TAG));

                string dataPath = args[0];
                decimal eurBalance = Convert.ToDecimal(args[1]);
                decimal btcBalance = Convert.ToDecimal(args[2]);
                string orderType = args[3];
                decimal orderAmount = Convert.ToDecimal(args[4]);
                MetaExchange metaExchange = new();
                Dictionary<string, OrderBook> orderbooks = metaExchange.GetOrderbooks(dataPath);
                List<Order> orders = metaExchange.CreateOrders(orderbooks.First().Value, eurBalance, btcBalance, orderType, orderAmount);
                string json = JsonConvert.SerializeObject(orders);
                Console.WriteLine(json);
            }
            catch (Exception e) { Console.WriteLine(string.Format("{0} ERROR: {1}", APP_TAG, e.Message)); }
            finally {  Console.WriteLine(string.Format("{0} Stopping service...", APP_TAG)); }
        }
    }
}