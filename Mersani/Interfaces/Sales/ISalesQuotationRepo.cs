using Mersani.models.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Sales
{
    public interface ISalesQuotationRepo
    {
 
        Task<DataSet> GetSalesquotationHdr(IsalesquotationMaster entity, string PostedType, string authParms);
        Task<DataSet> GetSalesquotationDetails(IsalesquotationDetails entity, string authParms);
        Task<DataSet> GetSalesquotationTerms(IsalesquotationTerms entity, string authParms);
        Task<DataSet> GetSalesquotationLastCode(string authParms);
        Task<DataSet> SalesquotationPosting(List<IsalesquotationMaster> entities, string authParms);
        Task<DataSet> SaveInvoicesHdrandDetails(ISalesQuotation entities, string authParms);
        Task<DataSet> DeleteSalesquotation(IsalesquotationMaster entities, string authParms);

    }
}
