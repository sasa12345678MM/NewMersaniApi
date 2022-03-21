using Mersani.models.Stock;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Stock
{
    public interface IInventoryItemsRepo
    {
        Task<DataSet> GetInventoryItems(InventoryItems entity, string authParms);
        Task<DataSet> GetInventoryItemsById(InventoryItems entity, string authParms);
        Task<DataSet> BulkInventoryItems(List<InventoryItems> entity, string authParms);
        Task<DataSet> DeleteInventoryItems(InventoryItems entity, string authParms);

        Task<DataSet> GetInventoryItemBatches(int stockId, int itemId, string authParms);
        Task<DataSet> GetInventoryByPharmacyId(string authParms);
    }
}
