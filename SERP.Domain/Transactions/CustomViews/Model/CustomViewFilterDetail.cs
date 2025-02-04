using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.CustomViews.Model
{
    public class CustomViewFilterDetail
    {
        public int custom_view_filter_id { get; set; }
        [StringLength(50)]
        public string filter { get; set; }
        [StringLength(1024)]
        public string filter_value { get; set; }
    }
}
