using System.Data;
using Mersani.models.Stock;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mersani.Interfaces.Stock
{
   public interface IItemBatchesRepo
    {
        Task<DataSet> GetItemBatches(ItemBatches entity, string authParms);
        Task<DataSet> BulkItemBatches(List<ItemBatches> entities, string authParms);
        Task<DataSet> DeleteItemBatches(ItemBatches entity, string authParms);
    }
}
