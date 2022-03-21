using Mersani.models.Stock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Stock
{
    public interface IInventoryLocationsRepo
    {
        Task<DataSet> GetInventoryLocations(InventoryLocations entity, string authParms);
        Task<DataSet> GetInventoryLocationsById(InventoryLocations entity, string authParms);
        Task<DataSet> PostInventoryLocations(InventoryLocations entity, string authParms);
        Task<DataSet> DeleteInventoryLocations(InventoryLocations entity, string authParms);

        Task<DataSet> GetLastCode(int id, string authParms);
    }
}
