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
    public class SalesOrderReturnRepository : ISalesOrderReturnRepo
    {
        public async Task<DataSet> GetSalesOrderReturnMaster(SalesOrderReturnMaster entity, string authParms)
        {
            var query = $"SELECT rtrn.*, ('أمر رقم ' || ordr.SOH_CODE || ' بتاريخ ' || ordr.SOH_DATE) AS ORDER_NAME_AR, ('Order No. ' || ordr.SOH_CODE || ' With Date ' || ordr.SOH_DATE) AS ORDER_NAME_EN," +
                $" FN_GET_SR_ORDER_TOTAL(rtrn.SROH_SYS_ID) AS GRAND_TOTAL " +
                $" FROM SALES_RTRN_ORDER_HDR rtrn " +
                $" JOIN SALES_ORDER_HDR ordr ON ordr.SOH_SYS_ID = rtrn.SROH_SOH_SYS_ID " +
                $" WHERE (SROH_SYS_ID = :pSROH_SYS_ID OR :pSROH_SYS_ID = 0) " +
                $" AND SROH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() { new OracleParameter("pSROH_SYS_ID", entity.SROH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetSalesOrderReturnDetails(SalesOrderReturnMaster entity, string authParms)
        {
            var query = $"SELECT * FROM SALES_RTRN_ORDER_DTL WHERE SROD_SROH_SYS_ID = :pSROD_SROH_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pSROD_SROH_SYS_ID", entity.SROH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetSalesOrderDetailsByCode(int code, string authParms)
        {
            var query = $"SELECT DTL.SOD_SYS_ID, DTL.SOD_SOH_SYS_ID, DTL.SOD_ITEM_SYS_ID, DTL.SOD_ITEM_QTY, DTL.SOD_ITEM_UOM_SYS_ID, DTL.SOD_ITEM_UNIT_PRICE, " +
                $" ROUND(FN_GET_ITEM_SALES_ORDER_PRICE(DTL.SOD_SYS_ID), 8) BASIC_PRICE, fn_get_ITEM_BASIC_UOM_QTY (DTL.SOD_ITEM_SYS_ID, DTL.SOD_ITEM_UOM_SYS_ID, DTL.SOD_ITEM_QTY) AS BASIC_QTY " +
                $" FROM SALES_ORDER_DTL DTL WHERE DTL.SOD_SOH_SYS_ID = :pSOD_SOH_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pSOD_SOH_SYS_ID", code) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetBasicItemQty(int itemCode, int unitCode, int qty, string authParms)
        {
            var query = $"SELECT  NVL (fn_get_ITEM_BASIC_UOM_QTY (:pItemCode, :pUnitCode, :pQty), 0) AS BASIC_QTY FROM DUAL";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pItemCode", itemCode),
                new OracleParameter("pUnitCode", unitCode),
                new OracleParameter("pQty", qty)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetSalesOrderReturnLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX (TO_NUMBER (SROH_CODE)), 0) + 1 AS Code FROM SALES_RTRN_ORDER_HDR WHERE SROH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostSalesOrderReturnMasterDetails(SalesOrderReturn entities, string authParms)
        {
            var authData = OracleDQ.GetAuthenticatedUserObject(authParms);

            //hdr
            entities.MASTER.SROH_V_CODE = authData.User_Act_PH;
            entities.MASTER.CURR_USER = authData.UserCode.Value;
            if (entities.MASTER.SROH_SYS_ID > 0) entities.MASTER.STATE = (int)OperationType.Update;
            else entities.MASTER.STATE = (int)OperationType.Add;

            // dtl 
            for (int i = 0; i < entities.DETAILS.Count; i++)
            {
                entities.DETAILS[i].SROD_SROH_SYS_ID = entities.MASTER.SROH_SYS_ID;
                entities.DETAILS[i].CURR_USER = authData.UserCode;
                if (entities.DETAILS[i].SROD_SYS_ID > 0) entities.DETAILS[i].STATE = (int)OperationType.Update;
                else entities.DETAILS[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.MASTER });
            parameters.Add("xml_document_d", entities.DETAILS.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_SALES_RTRN_ORDER_XML", parameters, authParms);
        }

        public async Task<DataSet> DeleteSalesOrderReturnMasterDetails(SalesOrderReturnDetails entity, int type, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteDeleteProcAsync("PRC_SALES_RTRN_ORDER_DEL", new { code = type == 1 ? entity.SROD_SROH_SYS_ID : entity.SROD_SYS_ID, type = type }, authParms);
        }


        /////////////////////////////////////help///////////////////////////////////
        public async Task<DataSet> BulkSalesApprovedOrders(List<SalesOrderReturnMaster> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                entity.STATE = (int)OperationType.Update;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                entity.SROH_V_CODE = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_S_RTRN_ORDR_APPROVE_XML", entities.ToList<dynamic>(), authParms);
        }


        public async Task<DataSet> GetNonApprovedOrders(SalesOrderReturnMaster entity, string authParms)
        {
            var query = $"SELECT * FROM SALES_RTRN_ORDER_HDR WHERE (SROH_SYS_ID = :pSROH_SYS_ID OR :pSROH_SYS_ID = 0) AND " +
                $" SROH_APPROVED_Y_N = 'Y' AND SROH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() { new OracleParameter("pSROH_SYS_ID", entity.SROH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }


    }
}
