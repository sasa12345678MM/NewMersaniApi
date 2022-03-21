using Mersani.models.Purchase;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Purchase
{
    public interface IPurchaseOrderRepo
    {
        Task<DataSet> GetPurchaseOrderMaster(PurchaseOrderMaster entity, string authParms);
        Task<DataSet> GetPurchaseOrderDetails(PurchaseOrderMaster entity, string authParms);
        Task<DataSet> GetPurchaseOrderLastCode(string authParms);
        Task<DataSet> GetOrderDetailsByRequest(int code, string authParms);
        Task<DataSet> PostPurchaseOrderMasterDetails(PurchaseOrder entities, string authParms);
        Task<DataSet> BulkPurchaseApprovedOrders(List<PurchaseOrderMaster> entities, string authParms);
        Task<DataSet> DeletePurchaseOrderMasterDetails(PurchaseOrderDetails entity, int type, string authParms);
        Task<DataSet> GetNonApprovedOrders(PurchaseOrderMaster entity, string authParms);
    }
}
