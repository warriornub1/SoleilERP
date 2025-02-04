using SERP.Application.Common;
using SERP.Domain.Transactions.Receiving;
using SERP.Domain.Transactions.Receiving.Model;
using System.Linq.Expressions;

namespace SERP.Application.Transactions.Receiving.Interfaces
{
    public interface IReceivingDetailRepository : IGenericRepository<ReceivingDetail>
    {
        Task<List<ReceivingDetail>> GetReceivingDetailListAsync(List<int> receivingDetailIds, int rcvHeaderId);
        Task<List<ReceivingDetail>> GetDetailByReceivingHeaderIdAsync(int rcvHeaderId);
        Task<int> GetReceivingDetailLastLineNoAsync(int rcvHeaderId);
        Task<List<string>> GetDocumentListAsync(int rcvHeaderId);
        Task<List<ReceivingItemModel>> GetItemListAsync(List<Expression<Func<ReceivingItemModel, bool>>> cond, int rcvHeaderId, string docNo, string packageNo);
        Task<List<ReceivingDetail>> GetReceivingDetailLineByItemAsync(List<Expression<Func<ReceivingDetail, bool>>> cond);
        Task<ReceivingItemDetailUomConversionModel> GetItemDetailsAsync(int rcvDetailId);
        Task<List<ReceivingItemDetailModel>> GetReceivingDetailsAsync(List<Expression<Func<ReceivingItemDetailModel, bool>>> cond);
    }
}
