namespace SERP.Domain.Masters.Currencies.Model
{
    public class CurrencyExchangeDetail
    {
        public int id { get; set; }
        public string from_currency { get; set; }
        public string to_currency { get; set; }
        public decimal exchange_rate { get; set; }
    }
}
