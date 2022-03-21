using System.Collections.Generic;
using System.Threading.Tasks;
using Mersani.models.Stock;
using System.Data;

namespace Mersani.Interfaces.Stock
{
    public interface IItemDosageRepo
    {
        Task<DataSet> GetItemDosages(StockItemDosage entity, string authParms);
        Task<DataSet> BulkItemDosages(List<StockItemDosage> entities, string authParms);
        Task<DataSet> DeleteItemDosage(StockItemDosage entity, string authParms);
    }
}
