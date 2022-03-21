using Mersani.models.Purchase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Purchase
{
    public interface PurchaseInvoicesReturnRepo
    {
        Task<DataSet> GetInvoicesReturnHdr(InvoicesReturnHead entity, string PostedType, string authParms);
        Task<DataSet> GetInvoicesReturnItem(InvoicesReturnItem entity, string authParms);
        Task<DataSet> GetInvoicesLastCode(string authParms);
        Task<DataSet> GetNonPostedInvoicesReturn(InvoicesReturnHead entity, string authParms);
        Task<DataSet> InvoicesReturnPosting(List<InvoicesReturnHead> entities, string authParms);
        Task<DataSet> GetDefaultAccountsForPurchase(InvoicesReturnHead entity, string authParms);
        Task<DataSet> SaveInvoicesHdrandItem(InvoiceReturnData entities, string authParms);
        Task<DataSet> DeleteInvoicesReturn(InvoicesReturnHead entities, string authParms);

    }
}
