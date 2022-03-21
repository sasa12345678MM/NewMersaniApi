using System.Data;
using Mersani.models.Stock;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Stock
{
    public interface ITransferRequestRepo
    {
        Task<DataSet> GetTransferRequestMaster(TransferRequestMaster entity, string authParms);
        Task<DataSet> GetTransferRequestDetails(TransferRequestMaster entity, string authParms);
        Task<DataSet> PostTransferRequestMasterDetails(TransferRequest entity, string authParms);
        Task<DataSet> GetTransferRequestLastCode(string authParms);
        Task<DataSet> DeleteTransferRequestMasterDetails(TransferRequestDetails entity, int type, string authParms);
    }
}
