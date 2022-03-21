using Mersani.Interfaces.CallCenter;
using Mersani.models.CostCenter;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.CallCenter
{
    public class TktSalesOrderRepository : TktSalesOrderRepo
    {
        public async Task<DataSet> GetSalesOrderDtl(TktSalesOrderDtl entity, string authParms)
        {
            var query = $"SELECT * FROM TKT_SALES_ORDER_DTL WHERE (TSOD_TSOH_SYS_ID=:PTSOD_TSOH_SYS_ID or :PTSOD_TSOH_SYS_ID = 0)";
            var parms = new List<OracleParameter>() { new OracleParameter("PTSOH_SYS_ID", entity.TSOD_TSOH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetTktSalesOrderHdr(TktSalesOrderHdr entity, string authParms)
        {
            var query = $"SELECT SOH.*, ph.PHARM_NAME_AR, ph.PHARM_NAME_EN, fcust.CUST_NAME_AR, fcust.CUST_NAME_EN " +
                $" FROM TKT_SALES_ORDER_HDR SOH" +
                $" INNER JOIN GAS_PHARMACY ph ON ph.PHARM_SYS_ID = SOH.TSOH_PH_SYS_ID" +
                $" INNER JOIN FINS_CUSTOMER fcust ON fcust.CUST_SYS_ID = SOH.TSOH_CUST_SYS_ID" +
                $" WHERE (TSOH_SYS_ID =:PTSOH_SYS_ID or: PTSOH_SYS_ID = 0)";          
            var parms = new List<OracleParameter>() { new OracleParameter("PTSOH_SYS_ID", entity.TSOH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> PostTktSalesOrderHdrDtl(TktSalesOrder entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            //entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
            entities.TKTSALESORDERHDR.CURR_USER = authP.UserCode.Value;
            //entities.TKTSALESORDERHDR.TSOH_CURRENT_STATUS_USR= authP.UserCode.Value;
            if (entities.TKTSALESORDERHDR.TSOH_SYS_ID > 0)
                entities.TKTSALESORDERHDR.STATE = (int)OperationType.Update;
            else entities.TKTSALESORDERHDR.STATE = (int)OperationType.Add;
            // DTL
            for (int i = 0; i < entities.TKTSALESORDERDTL.Count; i++)
            {
                entities.TKTSALESORDERDTL[i].CURR_USER = authP.UserCode;
                if (entities.TKTSALESORDERDTL[i].TSOD_SYS_ID > 0)
                    if (entities.TKTSALESORDERDTL[i].STATE == 3)
                    {
                        entities.TKTSALESORDERDTL[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.TKTSALESORDERDTL[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.TKTSALESORDERDTL[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.TKTSALESORDERHDR });
            parameters.Add("xml_document_d", entities.TKTSALESORDERDTL.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_TKT_SALES_ORDER_XML", parameters, authParms);
        }
        public async Task<DataSet> GetTktSalesOrderLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (TSOH_CODE, '^[0-9]+') THEN TSOH_CODE ELSE '0' END)), 0) + 1 AS Code " +
                $" from TKT_SALES_ORDER_HDR";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
        public async Task<DataSet> DeleteTktSalesOrderHdr(TktSalesOrderHdr entity, int v, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entity.CURR_USER = authP.UserCode.Value;
            entity.STATE = (int)OperationType.Delete;
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_Hdr", new List<dynamic>() { entity });
            parameters.Add("xml_document_Dtl", new List<dynamic>() { });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_TKT_SALES_ORDER_XML", parameters, authParms);
        }
        public async Task<DataSet> GetSalesOrderDetail(TktSalesOrderDetail entity, string authParms)
        {
            var query = $"select * from  TKT_SALES_ORDER_DETAIL  where TSOL_SOH_SYS_ID = :PTSOL_SOH_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PTSOL_SOH_SYS_ID", entity.TSOL_SOH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> SaveSalesOrderDetail(List<TktSalesOrderDetail> entity, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);

            for (int i = 0; i < entity.Count; i++)
            {
                entity[i].CURR_USER = authP.UserCode;
                if (entity[i].TSOL_SYS_ID > 0)
                    if (entity[i].STATE == 3)
                    {
                        entity[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entity[i].STATE = (int)OperationType.Update;
                    }
                else
                    entity[i].STATE = (int)OperationType.Add;
            }


            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_d", entity.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_TKT_SALES_ORDER_DETAIL", parameters, authParms);
        }
        public async Task<DataSet> GetTktUnSoldOrder(int id, string type, string authParms)
        {
            var query = $" SELECT SOH.*," +
                $"       ph.PHARM_NAME_AR," +
                $"       ph.PHARM_NAME_EN," +
                $"       fcust.CUST_NAME_AR," +
                $"       fcust.CUST_NAME_EN" +
                $"  FROM TKT_SALES_ORDER_HDR SOH" +
                $"       INNER JOIN GAS_PHARMACY ph ON ph.PHARM_SYS_ID = SOH.TSOH_PH_SYS_ID" +
                $"       INNER JOIN FINS_CUSTOMER fcust" +
                $"          ON fcust.CUST_SYS_ID = SOH.TSOH_CUST_SYS_ID" +
                $" WHERE SOH.TSOH_CURRENT_STATUS IN('WIC', 'TOC')";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PTSOH_SYS_ID", id)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> SaveSalesOrderStatus(List<TktSalesOrderHdr> entity, string authParms)
        {

            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            for (int i = 0; i < entity.Count; i++)
            {
                //entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
                entity[i].CURR_USER = authP.UserCode.Value;
                entity[i].TSOH_CURRENT_STATUS_USR= authP.UserCode.Value;
                entity[i].STATE = (int)OperationType.Update;
            }
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", entity.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_UP_TKT_S_ORDER_STAT_XML", parameters, authParms);
        }
        public async Task<DataSet> GetTktSalesOrderHdrLog(TktSalesOrderHdrLog entity, string authParms)
        {
            var query = $" select * from TKT_SALES_ORDER_HDR_LOG where TSOHL_TSOH_SYS_ID=:pTSOHL_TSOH_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pTSOHL_TSOH_SYS_ID", entity.TSOHL_TSOH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> CheckItemInStock(CheckInStockObj search, string authParms)
        {
            var query = $"  SELECT ITEM_SYS_ID, SUM (QTY_BASIC) TOTAL_QTY FROM(SELECT inv.IIM_INV_S_PHARM_SYS_ID, qty.ITEM_SYS_ID, qty.BASIC_UOM_SYS_ID, qty.QTY_BASIC " +
                $" FROM V_INV_ALL_QTY_SUM qty, inv_inventory_master inv WHERE qty.IIM_CODE = inv.IIM_CODE AND inv.IIM_INV_TYPE_I_S = 'S' AND qty.ITEM_SYS_ID = {search.ITEM_SYS_ID} " +
                $" AND IIM_INV_S_PHARM_SYS_ID = {search.PHARM_SYS_ID} AND qty.QTY_BASIC > 0) GROUP BY ITEM_SYS_ID";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetTktSalesOrderByCustomerId(int customerId, string authParms)
        {
            var query = $"SELECT SOH.*, addresses.FCA_ADDRES_DESC  , ph.PHARM_NAME_AR, ph.PHARM_NAME_EN, fcust.CUST_NAME_AR, fcust.CUST_NAME_EN " +
       $" FROM TKT_SALES_ORDER_HDR SOH" +
       $" INNER JOIN GAS_PHARMACY ph ON ph.PHARM_SYS_ID = SOH.TSOH_PH_SYS_ID" +
       $" INNER JOIN FINS_CUSTOMER fcust ON fcust.CUST_SYS_ID = SOH.TSOH_CUST_SYS_ID" +
       $" INNER JOIN FINS_CUSTOMER_ADDRESSES addresses ON addresses.FCA_SYS_ID = SOH.TSOH_CUST_ADDRSS_SYS_ID" +
       $" WHERE ( TSOH_CUST_SYS_ID ={customerId})";

            var query2 = $"select * from  TKT_SALES_ORDER_DTL join INV_ITEM_MASTER on ITEM_SYS_ID=TSOD_ITEM_SYS_ID  ";// where TSOL_SOH_SYS_ID = :PTSOL_SOH_SYS_ID";


           // var parms = new List<OracleParameter>() { new OracleParameter("TSOH_CUST_SYS_ID", customerId) };
            DataSet hdrs= await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);

            DataSet dtls = await OracleDQ.ExcuteGetQueryAsync(query2, null, authParms, CommandType.Text);

            return GetOrderHDrWithDetails(hdrs, dtls);
        }
        public DataSet GetOrderHDrWithDetails(DataSet hdrs, DataSet dtla)
        {
            DataTable hdrTable = hdrs.Tables["result"];
            DataTable dtlTable = dtla.Tables["result"];
            for (int i = 0; i < hdrTable.Rows.Count; i++)
            {
                DataRow row = hdrTable.Rows[i];
                decimal hdr_id = row.Field<decimal>("TSOH_SYS_ID");
                if (!hdrTable.Columns.Contains("dtls"))
                {
                    hdrTable.Columns.Add("dtls", typeof(List<TktSalesOrderDtl>));
                }
                DataView dv = new DataView(dtlTable);
                dv.RowFilter = $"TSOD_TSOH_SYS_ID = {hdr_id}";
                List<TktSalesOrderDtl> details = new List<TktSalesOrderDtl>();
                foreach (DataRowView drv in dv)
                {
                    decimal TSOD_SYS_ID = (decimal)drv["TSOD_SYS_ID"];
                    decimal TSOD_ITEM_SYS_ID = (decimal)drv["TSOD_ITEM_SYS_ID"];
                    string Item_Name_En = (string)drv["ITEM_NAME_EN"];
                    string Item_Name_ar = (string)drv["ITEM_NAME_AR"];
                       decimal TSOD_ITEM_QTY = (decimal)drv["TSOD_ITEM_QTY"];
                    details.Add(new TktSalesOrderDtl()
                    {
                        TSOD_ITEM_QTY = TSOD_ITEM_QTY,
                        TSOD_ITEM_SYS_ID =(int) TSOD_ITEM_SYS_ID,
                       TSOD_SYS_ID = (int)TSOD_SYS_ID,
                        ITEM_NAME_AR = Item_Name_ar,
                        ITEM_NAME_EN = Item_Name_En
                    }); 
                }

                row["dtls"] = details;
            }
            return hdrs;
        }

    }
}
