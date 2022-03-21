using Mersani.models.CostCenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.CallCenter
{
    public interface TktSalesOrderRepo
    {
        Task<DataSet> GetTktSalesOrderHdr(TktSalesOrderHdr entity, string authParms);
        Task<DataSet> GetSalesOrderDtl(TktSalesOrderDtl entity, string authParms);
        Task<DataSet> PostTktSalesOrderHdrDtl(TktSalesOrder entities, string authParms);
        Task<DataSet> DeleteTktSalesOrderHdr(TktSalesOrderHdr entity, int v, string authParms);
        Task<DataSet> GetTktSalesOrderLastCode(string authParms);
        //////////////////////////////////////////
        Task<DataSet> GetTktSalesOrderByCustomerId(int customerId, string authParms);

        Task<DataSet> GetSalesOrderDetail(TktSalesOrderDetail entity, string authParms);
        Task<DataSet> SaveSalesOrderDetail(List<TktSalesOrderDetail> entity, string authParms);

        //////////////////////////
        Task<DataSet> GetTktUnSoldOrder(int id,string type, string authParms);
        Task<DataSet> SaveSalesOrderStatus(List<TktSalesOrderHdr> entity, string authParms);

        ////////////////////////
        Task<DataSet> GetTktSalesOrderHdrLog(TktSalesOrderHdrLog entity, string authParms);

        Task<DataSet> CheckItemInStock(CheckInStockObj search, string authParms);
        
    }

    public class CheckInStockObj
    {
        public int? ITEM_SYS_ID { set; get; }
        public int? PHARM_SYS_ID { set; get; }
        public decimal? TOTAL_QTY { set; get; }
    }
}
