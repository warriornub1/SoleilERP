using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.SequencesTracking
{
    /// <summary>
    /// There will be a new table called SequenceTracking.
    /// There should be initial data for example seq_type "PO"
    /// Year and Month will be 2024 and 9 respectively
    /// 
    /// So you can create a common method to get record by seq_type.
    /// If the current year and month is the same as the record, get the next seq_no and update seq_no.
    /// If month is different, reset the seq_no to 1 and update month too.
    /// 
    /// year and month is optional. if it is 0, then year and month is not use to get the seq
    /// if month is 0 but year is not 0, then seq_no is by year.
    /// </summary>
    public class SequenceTracking : BaseModel
    {
        /// <summary>
        /// Type of Sequence. PO, SO, etc
        /// </summary>
        [StringLength(10)]
        public string seq_type { get; set; }
        public int? year { get; set; }
        public int? month { get; set; }
        /// <summary>
        /// Sequence No of Sequence Type. May include Year and Month in sequence.
        /// </summary>
        public int seq_no { get; set; }
    }
}
