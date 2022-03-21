using Mersani.models.Purchase;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Generic;

namespace Mersani.Interfaces.Purchase
{
    public interface IPurchasePaymentRepo
    {
        Task<DataSet> GetPurchasePaymentMaster(P_PaymentMaster entity, string authParms);
        Task<DataSet> GetPurchasePaymentDetails(P_PaymentMaster entity, string authParms);
        Task<DataSet> GetPaymentLastCode(string authParms);
        Task<DataSet> PostPurchasePaymentMasterDetails(PurchasePayment entities, string authParms);
        Task<DataSet> DeletePurchasePaymentMasterDetails(P_PaymentDetails entity, int type, string authParms);

        Task<DataSet> BulkPurchaseApprovedPayments(List<P_PaymentMaster> entities, string authParms);
    }
}
