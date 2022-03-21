using System.Collections.Generic;
using System.Threading.Tasks;
using Mersani.models.Stock; 
using System.Data;

namespace Mersani.Interfaces.Stock
{
    public interface IItemManufacturerRepo
    {
        Task<DataSet> GetItemManufacturers(StockItemManufacturer entity, string authParms);
        Task<DataSet> BulkItemManufacturers(List<StockItemManufacturer> entities, string authParms);
        Task<DataSet> DeleteItemManufacturer(StockItemManufacturer entity, string authParms);
    }
}

