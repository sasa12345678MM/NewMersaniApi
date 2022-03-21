using Mersani.models.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Sales
{
    public interface ISalesOrderReturnRepo
    {
        Task<DataSet> GetSalesOrderReturnMaster(SalesOrderReturnMaster entity, string authParms);
        Task<DataSet> GetSalesOrderReturnDetails(SalesOrderReturnMaster entity, string authParms);
        Task<DataSet> GetSalesOrderReturnLastCode(string authParms);
        Task<DataSet> GetSalesOrderDetailsByCode(int code, string authParms);
        Task<DataSet> PostSalesOrderReturnMasterDetails(SalesOrderReturn entities, string authParms);
        Task<DataSet> BulkSalesApprovedOrders(List<SalesOrderReturnMaster> entities, string authParms);
        Task<DataSet> DeleteSalesOrderReturnMasterDetails(SalesOrderReturnDetails entity, int type, string authParms);
        Task<DataSet> GetNonApprovedOrders(SalesOrderReturnMaster entity, string authParms);
        Task<DataSet> GetBasicItemQty(int itemCode, int unitCode, int qty, string authParms);
    }
}
