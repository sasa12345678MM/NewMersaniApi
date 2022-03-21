using Mersani.models.PointOfSale;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.PointOfSale
{
    public interface IPosRequestItemsRepo
    {
        Task<DataSet> GetPosRequestItemsMaster(PosRequestItemsMaster entity, string authParms);
        Task<DataSet> GetPosRequestItemsMasterForPending(PosRequestItemsMaster entity, string authParms);
        Task<DataSet> GetPosRequestItemsDetails(PosRequestItemsMaster entity, string authParms);
        Task<DataSet> PostPosRequestItemsMasterDetails(PosRequestItems entity, string authParms);
        Task<DataSet> GetPosRequestItemsLastCode(string authParms);
        Task<DataSet> DeletePosRequestItemsMasterDetails(PosRequestItemsDetails entity, int type, string authParms);
        Task<DataSet> GetPosRequestItemsPendingForApproval(PosRequestItemsMaster entity, string authParms);
        Task<DataSet> GetPosRequestItemsPendigForConfirm(PosRequestItemsMaster entity, string authParms);


    }
}
