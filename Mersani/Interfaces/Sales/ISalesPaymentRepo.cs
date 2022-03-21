using System.Threading.Tasks;
using Mersani.models.Sales;
using System.Data;
using System.Collections.Generic;

namespace Mersani.Interfaces.Sales
{
    public interface ISalesPaymentRepo
    {
        Task<DataSet> GetSalesPaymentMaster(S_PaymentMaster entity, string authParms);
        Task<DataSet> GetSalesPaymentDetails(S_PaymentMaster entity, string authParms);
        Task<DataSet> GetPaymentLastCode(string authParms);
        Task<DataSet> PostSalesPaymentMasterDetails(SalesPayment entities, string authParms);
        Task<DataSet> DeleteSalesPaymentMasterDetails(S_PaymentDetails entity, int type, string authParms);

        Task<DataSet> BulkSalesApprovedPayments(List<S_PaymentMaster> entities, string authParms);
    }
}
