using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.CustomViews.Model
{
    public class UpdateCustomViewRequestModel
    {
        public int id { get; set; }
        public List<UpdateAttributeViewModel> attributes { get; set; }
        public List<UpdateCustomViewFilterModel>? filters { get; set; }
    }

    public class UpdateAttributeViewModel : AttributeViewModel
    {
        public int custom_view_attribute_id { get; set; }

        private string _action;

        [StringLength(1)]
        [AcceptValue("C", "U", "D")]
        public string action
        {
            get => _action;
            set => _action = value.ToUpper();
        }
    }

    public class UpdateCustomViewFilterModel : CustomViewFilterRequestModel
    {
        public int custom_view_filter_id { get; set; }
        private string _action;

        [StringLength(1)]
        [AcceptValue("C", "U", "D")]
        public string action
        {
            get => _action;
            set => _action = value.ToUpper();
        }
    }
}
