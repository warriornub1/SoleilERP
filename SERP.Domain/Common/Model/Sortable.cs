namespace SERP.Domain.Common.Model
{
    public class Sortable
    {
        
        public string FieldName { get; set; }
        public bool IsAscending { get; set; }

        public Sortable(string fieldName, bool isAscending)
        {
            FieldName = fieldName;
            IsAscending = isAscending;
        }

        public Sortable()
        {
        }
    }
}
