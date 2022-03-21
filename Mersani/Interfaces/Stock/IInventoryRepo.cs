using System.Data;
using Mersani.models.Stock;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Stock
{
    public interface IInventoryRepo
    {
        Task<DataSet> GetInventoryData(Inventory inventory, string authParms);
        Task<DataSet> GetInventoryDataById(int invId, string authParms);
        Task<DataSet> PostInventoryData(Inventory inventory, string authParms);
        Task<DataSet> DeleteInventoryData(Inventory inventory, string authParms);

        Task<DataSet> GetLastCode(string authParms);
    }
}
