using System.Data;
using Mersani.models.Stock;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Stock
{
    public interface IInventoryTransferRepo
    {
        Task<DataSet> GetTransferMaster(TransferMaster entity, string authParms);
        Task<DataSet> GetTransferDetails(TransferMaster entity, string authParms);
        Task<DataSet> PostTransferMasterDetails(InventoryTransfer entities, string authParms);
        Task<DataSet> DeleteTransferMasterDetails(TransferDetails entity, int type, string authParms);
        Task<DataSet> GetTransferDetailsByRequest(int code, string authParms);
        Task<DataSet> GetTransferLastCode(int id, string authParms);
    }
}
