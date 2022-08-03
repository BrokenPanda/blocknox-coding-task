using meta_exchange;
using meta_exchange.models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace meta_exchange_api.Controllers
{
    [ApiController]
    [Route("order-create")]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public string Get() { return "missing parameters."; }

        // GET order-create/{eurBalance}/{btcBalance}/{orderType}/{orderAmount}
        // GET order-create/25000/0/Buy/5
        [HttpGet("{eurBalance}/{btcBalance}/{orderType}/{orderAmount}")]
        public string GetOrders(decimal eurBalance, decimal btcBalance, string orderType, decimal orderAmount)
        {
            try
            {
                MetaExchange metaExchange = new();
                Dictionary<string, OrderBook> orderBooks = metaExchange.GetOrderbooks(Environment.CurrentDirectory + "/order_books_data.json");
                List<Order> orders = metaExchange.CreateOrders(orderBooks.First().Value, eurBalance, btcBalance, orderType, orderAmount);
                string json = JsonConvert.SerializeObject(orders);
                return json;
            }
            catch (Exception e)
            {
                throw new ArgumentException(string.Format("ERROR: {0}", e.Message));
            }
        }
    }
}
