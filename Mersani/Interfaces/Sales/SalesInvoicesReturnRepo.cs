using Mersani.models.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Sales
{
    public interface SalesInvoicesReturnRepo
    {
        Task<DataSet> GetSalesInvoicesReturnHdr(SalesInvoicesReturnHead entity, string PostedType, string authParms);
        Task<DataSet> GetSalesInvoicesReturnItem(SalesInvoicesReturnItem entity, string authParms);
        Task<DataSet> GetInvoicesLastCode(string authParms);
        Task<DataSet> GetNonPostedSalesInvoicesReturn(SalesInvoicesReturnHead entity, string authParms);
        Task<DataSet> SalesInvoicesReturnPosting(List<SalesInvoicesReturnHead> entities, string authParms);
        Task<DataSet> GetDefaultAccountsForPurchase(SalesInvoicesReturnHead entity, string authParms);
        Task<DataSet> SaveInvoicesHdrandItem(SalesInvoiceReturnData entities, string authParms);
        Task<DataSet> DeleteSalesInvoicesReturn(SalesInvoicesReturnHead entities, string authParms);

    }
}
