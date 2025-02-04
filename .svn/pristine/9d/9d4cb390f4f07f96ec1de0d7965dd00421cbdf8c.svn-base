using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Domain.Masters.Currencies
{
    public class CurrencyExchange: BaseModel
    {
        public int base_currency_id { get; set; }
        public int currency_id { get; set; }
        [Column(TypeName = "decimal(13,7)")]
        public decimal exchange_rate { get; set; }
    }
}
