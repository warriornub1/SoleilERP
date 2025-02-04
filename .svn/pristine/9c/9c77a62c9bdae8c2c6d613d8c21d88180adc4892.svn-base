using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.SequencesTracking.Interfaces;
using SERP.Domain.Transactions.SequencesTracking;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Transactions.SequencesTracking
{
    internal class SequenceTrackingRepository : GenericRepository<SequenceTracking>, ISequenceTrackingRepository
    {
        public SequenceTrackingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> GetSequenceNoByType(string type)
        {
            var currentTime = DateTime.Now;
            var lastSeq = await _dbContext.SequenceTracking
                .FirstOrDefaultAsync(x => x.seq_type == type);

            // initial data
            if (lastSeq == null)
            {
                lastSeq = new SequenceTracking
                {
                    seq_no = 1,
                    seq_type = type,
                    year = currentTime.Year,
                    month = currentTime.Month,
                    created_on = currentTime,
                    created_by = "System"
                };

                _dbContext.SequenceTracking.Add(lastSeq);
                await _dbContext.SaveChangesAsync();

                return lastSeq.seq_no;
            }

            // If the current year and month is the same as the record, get the next seq_no and update seq_no.
            if (lastSeq.year == currentTime.Year && lastSeq.month == currentTime.Month)
            {
                lastSeq.seq_no += 1;
                lastSeq.last_modified_on = currentTime;
            }
            // If month is different, reset the seq_no to 1 and update month too.
            else
            {
                lastSeq.seq_no = 1;
                lastSeq.month = currentTime.Month;
                lastSeq.year = currentTime.Year;
                lastSeq.last_modified_on = currentTime;
                lastSeq.last_modified_by = "System";
            }

            _dbContext.SequenceTracking.Update(lastSeq);
            await _dbContext.SaveChangesAsync();

            // year and month is optional. if it is 0, then year and month is not use to get the seq
            // TODO: if month is 0 but year is not 0, then seq_no is by year.

            return lastSeq.seq_no;
        }
    }
}
