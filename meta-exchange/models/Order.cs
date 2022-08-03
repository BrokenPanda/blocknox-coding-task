namespace meta_exchange.models
{
    public class Order
    {
        public int? Id { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public string Kind { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
    }
}
