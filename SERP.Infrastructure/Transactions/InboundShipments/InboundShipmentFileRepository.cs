using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.InboundShipments.Interfaces;
using SERP.Domain.Transactions.InboundShipments;
using SERP.Domain.Transactions.InboundShipments.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Transactions.InboundShipments
{
    internal class InboundShipmentFileRepository : GenericRepository<InboundShipmentFile>, IInboundShipmentFileRepository
    {
        public InboundShipmentFileRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<InboundShipmentFileDetail>?> GetInboundShipmentFileDetail(int inboundShipmentId)
        {
            var result = await (from inboundShipmentFile in _dbContext.InboundShipmentFiles.AsNoTracking()
                                join fileTracking in _dbContext.FilesTracking.AsNoTracking() on inboundShipmentFile
                                    .file_id equals fileTracking.id
                                where inboundShipmentFile.inbound_shipment_id == inboundShipmentId
                                select new InboundShipmentFileDetail
                                {
                                    id = inboundShipmentFile.id,
                                    file_type = fileTracking.file_type,
                                    document_type = fileTracking.document_type,
                                    created_by = inboundShipmentFile.created_by,
                                    created_on = inboundShipmentFile.created_on
                                }).ToListAsync();

            return result;
        }

        public async Task<List<InboundShipmentFile>?> GetInboundShipmentFiles(int inboundShipmentId)
        {
            return await _dbContext.InboundShipmentFiles.AsNoTracking().Where(x => x.inbound_shipment_id == inboundShipmentId).ToListAsync();
        }
    }
}
