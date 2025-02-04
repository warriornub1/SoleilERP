using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.AdvancedShipmentNotices.Interfaces;
using SERP.Domain.Transactions.AdvancedShipmentNotices;
using SERP.Domain.Transactions.AdvancedShipmentNotices.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Transactions.AdvancedShipmentNotices
{
    internal class ASNFileRepository: GenericRepository<ASNFile>, IASNFileRepository
    {
        public ASNFileRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<AsnFileResponseDetail>> GetFileInfoAsync(int asnHeaderId)
        {
            var query = from asnFile in _dbContext.AsnFiles
                join fileTracking in _dbContext.FilesTracking on asnFile.file_id equals fileTracking.id
                where asnFile.asn_header_id == asnHeaderId
                select new AsnFileResponseDetail
                {
                    id = asnFile.id,
                    document_type = fileTracking.document_type,
                    file_name = fileTracking.file_name,
                    url_path = fileTracking.url_path,
                    file_type = fileTracking.file_type,
                    created_by = asnFile.created_by,
                    created_on = asnFile.created_on
                };

            return await query.ToListAsync();
        }

        public async Task<Dictionary<int, bool>> HasAttachmentCheck(HashSet<int> asnHeaderIds)
        {
            var result = await _dbContext.InvoiceFiles.AsNoTracking()
                .Where(x => asnHeaderIds.Contains(x.invoice_header_id))
                .GroupBy(x => x.invoice_header_id)
                .ToDictionaryAsync(x => x.Key, x => true);
            return result;
        }
    }
}
