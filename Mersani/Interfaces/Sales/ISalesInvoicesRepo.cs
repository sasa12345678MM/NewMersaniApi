using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mersani.models.Sales;

namespace Mersani.Interfaces.Sales
{
    public interface ISalesInvoicesRepo
    {
        Task<DataSet> GetInvoicesMaster(SalesInvoices entity, string authParms);
        Task<DataSet> GetInvoicesDetails(SalesInvoices entity, string authParms);
        Task<DataSet> GetInvoicesLastCode(string authParms);
        Task<DataSet> GetNonPostedInvoices(SalesInvoices entity, string authParms);
        Task<DataSet> GetDefaultAccountsForSales(SalesInvoices entity, string authParms);
        Task<DataSet> PostInvoicesMasterDetails(SalesInvoicesData entities, string authParms);
        Task<DataSet> BulkSalesPostingInvoices(List<SalesInvoices> entity, string authParms);
        Task<DataSet> DeleteInvoicesMasterDetails(SalesInvoiceItems entity, int type, string authParms);
    }
}
