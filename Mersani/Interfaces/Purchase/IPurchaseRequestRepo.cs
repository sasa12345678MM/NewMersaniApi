using Mersani.models.Purchase;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Purchase
{
    public interface IPurchaseRequestRepo
    {
        Task<DataSet> GetPurchaseRequestMaster(PurchaseRequestMaster entity, string authParms);
        Task<DataSet> GetPurchaseRequestDetails(PurchaseRequestMaster entity, string authParms);
        Task<DataSet> PostPurchaseRequestMasterDetails(PurchaseRequest entity, string authParms);
        Task<DataSet> GetPurchaseRequestLastCode(string authParms);
        Task<DataSet> DeletePurchaseRequestMasterDetails(PurchaseRequestDetails entity, int type, string authParms);

        Task<DataSet> GetOwnerApprovedRequestMaster(PurchaseRequestMaster entity, string authParms);
        Task<DataSet> GetCompanyApprovedRequestMaster(PurchaseRequestMaster entity, string authParms);

        Task<DataSet> GetRequestsForDashboard(PurchaseRequestDashboard criteria, string authParms);
        Task<DataSet> GetPurchaseBasicQty(PurchaseRequestDetails criteria, string authParms);
    }
}
