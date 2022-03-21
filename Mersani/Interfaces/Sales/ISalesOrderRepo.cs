using Mersani.models.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Sales
{
    public interface ISalesOrderRepo
    {
        Task<DataSet> GetSalesOrderMaster(SalesOrderMaster entity, string authParms);
        Task<DataSet> GetSalesOrderDetails(SalesOrderMaster entity, string authParms);
        Task<DataSet> GetSalesOrderLastCode(string authParms);
        Task<DataSet> GetOrderDetailsByQuotation(int code, string authParms);
        Task<DataSet> PostSalesOrderMasterDetails(SalesOrder entities, string authParms);
        Task<DataSet> BulkSalesApprovedOrders(List<SalesOrderMaster> entities, string authParms);
        Task<DataSet> DeleteSalesOrderMasterDetails(SalesOrderDetails entity, int type, string authParms);
        Task<DataSet> GetNonApprovedOrders(SalesOrderMaster entity, string authParms);
    }
}
