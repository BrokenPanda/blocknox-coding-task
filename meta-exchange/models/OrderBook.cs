namespace meta_exchange.models
{
    public class OrderBook
    {
        public DateTime AcqTime { get; set; }
        public List<Bid> Bids { get; set; }
        public List<Ask> Asks { get; set; }
    }
}