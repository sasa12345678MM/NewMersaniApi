using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mersani.models.Purchase;

namespace Mersani.Interfaces.Purchase
{
    public interface IPurchaseInvoicesRepo
    {
        Task<DataSet> GetInvoicesMaster(PurchaseInvoices entity, string authParms);
        Task<DataSet> GetInvoicesMasterDataSearch(dynamic entity, string authParms);
        Task<DataSet> GetInvoicesDetails(PurchaseInvoices entity, string authParms);
        Task<DataSet> GetInvoicesLastCode(string authParms);
        Task<DataSet> GetNonPostedInvoices(PurchaseInvoices entity, string authParms);
        Task<DataSet> GetDefaultAccountsForPurchase(PurchaseInvoices entity, string authParms);
        Task<DataSet> BulkPurchasePostingInvoices(List<PurchaseInvoices> entities, string authParms);
        Task<DataSet> PostInvoicesMasterDetails(PurchaseInvoicesData entities, string authParms);
        Task<DataSet> DeleteInvoicesMasterDetails(PurchaseInvoiceItems entity, int type, string authParms);
    }
}
