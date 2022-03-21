using Mersani.Interfaces.Sales;
using Mersani.models.Sales;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Sales
{
    public class SalesOrderRepository : ISalesOrderRepo
    {
        public async Task<DataSet> GetSalesOrderMaster(SalesOrderMaster entity, string authParms)
        {
            var query = $"SELECT ordr.*, cust.CUST_NAME_AR AS SOH_CUST_NAME_AR, cust.CUST_NAME_EN SOH_CUST_NAME_EN, ownr.OWNER_NAME_AR AS SOH_OWNER_NAME_AR,ownr.OWNER_NAME_EN SOH_OWNER_NAME_EN," +
                $" FN_GET_SALES_ORDER_TOTAL(ordr.SOH_SYS_ID) AS GRAND_TOTAL " +
                $" FROM SALES_ORDER_HDR ordr " +
                $" LEFT JOIN FINS_CUSTOMER cust ON cust.CUST_SYS_ID = ordr.SOH_OWNER_CUST_SYS_ID " +
                $" LEFT JOIN GAS_OWNER ownr ON ownr.OWNER_SYS_ID = ordr.SOH_OWNER_CUST_SYS_ID " +
                $" WHERE (SOH_SYS_ID = :pSOH_SYS_ID OR :pSOH_SYS_ID = 0) AND SOH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() { new OracleParameter("pSOH_SYS_ID", entity.SOH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetSalesOrderDetails(SalesOrderMaster entity, string authParms)
        {
            var query = $"SELECT dtl.*, itm.ITEM_NAME_AR, itm.ITEM_NAME_EN, unt.UOM_NAME_AR, unt.UOM_NAME_EN, ROUND (NVL (fn__item_btch_curr_stk (hdr.SOH_INV_SYS_ID, dtl.SOD_ITEM_SYS_ID, NULL, dtl.SOD_ITEM_UOM_SYS_ID), 0), 8) AS CURR_STK " +
                $" FROM SALES_ORDER_DTL dtl " +
                $" JOIN SALES_ORDER_HDR hdr ON hdr.SOH_SYS_ID = dtl.SOD_SOH_SYS_ID " +
                $" JOIN INV_ITEM_MASTER itm ON dtl.SOD_ITEM_SYS_ID = itm.ITEM_SYS_ID " +
                $" JOIN INV_UOM unt ON dtl.SOD_ITEM_UOM_SYS_ID = unt.UOM_SYS_ID " +
                $" WHERE SOD_SOH_SYS_ID = :pSOD_SOH_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pSOD_SOH_SYS_ID", entity.SOH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetOrderDetailsByQuotation(int code, string authParms)
        {
            var result = OracleDQ.GetData<dynamic>("SELECT * FROM SALES_QUOTATION_DTL WHERE SQD_SQH_SYS_ID = :pCode", authParms, new { pCode = code }, CommandType.Text);
            var query = $"SELECT qdtl.*, itms.III_INV_SYS_ID, ROUND({result.Count}, 8) AS ITEMS_COUNT, ROUND(NVL(fn__item_btch_curr_stk (itms.III_INV_SYS_ID, qdtl.SQD_ITEM_SYS_ID, NULL, qdtl.SQD_ITEM_UOM_SYS_ID), 0), 8) AS CURR_STK_QTY " +
                $" FROM SALES_QUOTATION_DTL qdtl " +
                $" left join inv_inv_items itms on qdtl.SQD_ITEM_SYS_ID = itms.III_ITEM_SYS_ID " +
                $" WHERE qdtl.SQD_SQH_SYS_ID = :pCode";
            var parms = new List<OracleParameter>() { new OracleParameter("pCode", code) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetSalesOrderLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX (TO_NUMBER (SOH_CODE)), 0) + 1 AS Code FROM SALES_ORDER_HDR WHERE SOH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostSalesOrderMasterDetails(SalesOrder entities, string authParms)
        {
            var authData = OracleDQ.GetAuthenticatedUserObject(authParms);

            //hdr
            entities.MASTER.SOH_V_CODE = authData.User_Act_PH;
            entities.MASTER.CURR_USER = authData.UserCode.Value;
            if (entities.MASTER.SOH_SYS_ID > 0) entities.MASTER.STATE = (int)OperationType.Update;
            else entities.MASTER.STATE = (int)OperationType.Add;

            // dtl 
            for (int i = 0; i < entities.DETAILS.Count; i++)
            {
                entities.DETAILS[i].SOD_SOH_SYS_ID = entities.MASTER.SOH_SYS_ID;
                entities.DETAILS[i].CURR_USER = authData.UserCode;
                if (entities.DETAILS[i].SOD_SYS_ID > 0) entities.DETAILS[i].STATE = (int)OperationType.Update;
                else entities.DETAILS[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.MASTER });
            parameters.Add("xml_document_d", entities.DETAILS.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_SALES_ORDER_XML", parameters, authParms);
        }

        public async Task<DataSet> DeleteSalesOrderMasterDetails(SalesOrderDetails entity, int type, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteDeleteProcAsync("PRC_SALES_ORDER_DEL", new { code = type == 1 ? entity.SOD_SOH_SYS_ID : entity.SOD_SYS_ID, type = type }, authParms);
        }


        /////////////////////////////////////help///////////////////////////////////
        public async Task<DataSet> BulkSalesApprovedOrders(List<SalesOrderMaster> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                entity.STATE = (int)OperationType.Update;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                entity.SOH_V_CODE = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_SALES_ORDER_APPROVE_XML", entities.ToList<dynamic>(), authParms);
        }


        public async Task<DataSet> GetNonApprovedOrders(SalesOrderMaster entity, string authParms)
        {
            var query = $"SELECT ordr.*, cust.CUST_NAME_AR AS SOH_CUST_NAME_AR, cust.CUST_NAME_EN SOH_CUST_NAME_EN, ownr.OWNER_NAME_AR AS SOH_OWNER_NAME_AR,ownr.OWNER_NAME_EN SOH_OWNER_NAME_EN " +
                $" FROM SALES_ORDER_HDR ordr " +
                $" LEFT JOIN FINS_CUSTOMER cust ON cust.CUST_SYS_ID = ordr.SOH_OWNER_CUST_SYS_ID " +
                $" LEFT JOIN GAS_OWNER ownr ON ownr.OWNER_SYS_ID = ordr.SOH_OWNER_CUST_SYS_ID " +
                $" WHERE (SOH_SYS_ID = :pSOH_SYS_ID OR :pSOH_SYS_ID = 0) AND SOH_APPROVED_Y_N = 'N' AND SOH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() { new OracleParameter("pSOH_SYS_ID", entity.SOH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }


    }
}
