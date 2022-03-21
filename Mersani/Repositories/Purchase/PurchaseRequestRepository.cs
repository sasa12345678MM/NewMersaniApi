using Oracle.ManagedDataAccess.Client;
using Mersani.Interfaces.Purchase;
using System.Collections.Generic;
using Mersani.models.Purchase;
using System.Threading.Tasks;
using Mersani.Oracle;
using System.Linq;
using System.Data;
using Mersani.Utility;

namespace Mersani.Repositories.Purchase
{
    public class PurchaseRequestRepository : IPurchaseRequestRepo
    {
        public async Task<DataSet> GetPurchaseRequestMaster(PurchaseRequestMaster entity, string authParms)
        {
            var query = $"SELECT * FROM INV_PRCH_REQST_HDR WHERE (IPRH_SYS_ID = :pSYS_ID OR :pSYS_ID = 0) AND IPRH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", entity.IPRH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetPurchaseRequestDetails(PurchaseRequestMaster entity, string authParms)
        {
            var query = $"SELECT rqst.*, item.ITEM_NAME_AR, item.ITEM_NAME_EN, NVL(fn_get_item_lpur_price(ITEM.ITEM_SYS_ID, ITEM.ITEM_UOM_SYS_ID, 1, {OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0) AS IPRD_ITEM_COST, unit.UOM_NAME_AR, unit.UOM_NAME_EN, ownr.OWNER_NAME_AR, ownr.OWNER_NAME_EN, " +
                $" fn_get_ITEM_BASIC_UOM_QTY (IPRD_ITEM_SYS_ID, IPRD_UOM_SYS_ID, IPRD_QTY) AS IPRD_BASIC_QTY, " +
                $" fn_get_ITEM_BASIC_UOM_QTY (IPRD_ITEM_SYS_ID, IPRD_UOM_SYS_ID, nvl(IPRD_OWNR_APPROVED_QTY,IPRD_QTY)) AS IPRD_OWNR_BASIC_QTY, " +
                $" fn_get_ITEM_BASIC_UOM_QTY (IPRD_ITEM_SYS_ID, IPRD_UOM_SYS_ID, nvl(IPRD_CPM_APPROVED_QTY,IPRD_OWNR_APPROVED_QTY)) AS IPRD_CPM_BASIC_QTY, " +
                $" (fn_get_ITEM_BASIC_UOM_QTY (IPRD_ITEM_SYS_ID, IPRD_UOM_SYS_ID, IPRD_QTY) * NVL(fn_get_item_lpur_price(ITEM.ITEM_SYS_ID, ITEM.ITEM_UOM_SYS_ID, 1, {OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0)) AS IPRD_TOTAL, " +
                $" (fn_get_ITEM_BASIC_UOM_QTY (IPRD_ITEM_SYS_ID, IPRD_UOM_SYS_ID, nvl(IPRD_OWNR_APPROVED_QTY,IPRD_QTY)) *NVL(fn_get_item_lpur_price(ITEM.ITEM_SYS_ID, ITEM.ITEM_UOM_SYS_ID, 1, {OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0)) AS IPRD_OWNR_TOTAL, " +
                $" (fn_get_ITEM_BASIC_UOM_QTY (IPRD_ITEM_SYS_ID, IPRD_UOM_SYS_ID, nvl(IPRD_CPM_APPROVED_QTY,IPRD_OWNR_APPROVED_QTY)) * NVL(fn_get_item_lpur_price(ITEM.ITEM_SYS_ID, ITEM.ITEM_UOM_SYS_ID, 1, {OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0)) AS IPRD_CPM_TOTAL " +
                $" FROM INV_PRCH_REQST_DTL rqst " +
                $" join inv_item_master item on item.ITEM_SYS_ID = rqst.IPRD_ITEM_SYS_ID " +
                $" left join inv_uom unit on unit.UOM_SYS_ID = rqst.IPRD_UOM_SYS_ID " +
                $" left join gas_owner ownr on ownr.OWNER_SYS_ID = rqst.IPRD_CPM_FRM_OWNR_SYS_ID " +
                $" WHERE IPRD_IPRH_SYS_ID = :pIPRD_IPRH_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pIPRD_IPRH_SYS_ID", entity.IPRH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostPurchaseRequestMasterDetails(PurchaseRequest entity, string authParms)
        {
            var authData = OracleDQ.GetAuthenticatedUserObject(authParms);

            //hdr
            entity.MASTER.IPRH_V_CODE = authData.User_Act_PH;
            entity.MASTER.CURR_USER = authData.UserCode.Value;
            if (entity.MASTER.IPRH_SYS_ID > 0) entity.MASTER.STATE = (int)OperationType.Update;
            else entity.MASTER.STATE = (int)OperationType.Add;

            // dtl 
            for (int i = 0; i < entity.DETAILS.Count; i++)
            {
                entity.DETAILS[i].IPRD_IPRH_SYS_ID = entity.MASTER.IPRH_SYS_ID;
                entity.DETAILS[i].CURR_USER = authData.UserCode;
                if (entity.DETAILS[i].IPRD_SYS_ID > 0) entity.DETAILS[i].STATE = (int)OperationType.Update;
                else entity.DETAILS[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entity.MASTER });
            parameters.Add("xml_document_d", entity.DETAILS.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_PRCH_REQST_XML", parameters, authParms);
        }

        public async Task<DataSet> DeletePurchaseRequestMasterDetails(PurchaseRequestDetails entity, int type, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteDeleteProcAsync("PRC_INV_PRCH_REQST_DEL", new { code = type == 1 ? entity.IPRD_IPRH_SYS_ID : entity.IPRD_SYS_ID, type = type }, authParms);
        }

        public async Task<DataSet> GetPurchaseRequestLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (IPRH_CODE, '^[0-9]+') THEN IPRH_CODE ELSE '0' END)), 0) + 1 AS Code FROM INV_PRCH_REQST_HDR WHERE IPRH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetOwnerApprovedRequestMaster(PurchaseRequestMaster entity, string authParms)
        {
            var query = $"SELECT * FROM INV_PRCH_REQST_HDR WHERE (IPRH_SYS_ID = :pSYS_ID OR :pSYS_ID = 0) AND IPRH_APPROVED_Y_N = 'Y' AND IPRH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", entity.IPRH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetCompanyApprovedRequestMaster(PurchaseRequestMaster entity, string authParms)
        {
            var query = $"SELECT * FROM INV_PRCH_REQST_HDR WHERE (IPRH_SYS_ID = :pSYS_ID OR :pSYS_ID = 0) AND IPRH_OWNR_APPROVED_Y_N = 'Y' AND IPRH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", entity.IPRH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetRequestsForDashboard(PurchaseRequestDashboard criteria, string authParms)
        {
            var query = $"SELECT * FROM V_INV_PRCH_REQST_HDR WHERE IPRH_APPROVED_Y_N = 'Y' AND IPRH_OWNR_APPROVED_Y_N = 'Y' AND IPRH_CPM_APPROVED_Y_N = 'Y' AND IPRH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}' ";
            if (criteria.ITEM_CODE != null) query += $" AND ITEM_CODE = :pITEM_CODE ";
            if (criteria.IPRH_CODE != null) query += $" AND IPRH_CODE = :pIPRH_CODE ";
            if (criteria.IPRH_DATE != null) query += $" AND IPRH_DATE = TO_DATE(:pIPRH_DATE, 'DD-MM-YYYY') ";

            if (criteria.IPRD_ITEM_SYS_ID != null) query += $" AND IPRD_ITEM_SYS_ID = :pIPRD_ITEM_SYS_ID ";
            if (criteria.ITEM_BASIC_SUPP_SYS_ID != null) query += $" AND ITEM_BASIC_SUPP_SYS_ID = :pITEM_BASIC_SUPP_SYS_ID ";
            if (criteria.IPRD_CPM_FRM_OWNR_SYS_ID != null) query += $" AND IPRD_CPM_FRM_OWNR_SYS_ID = :pIPRD_CPM_FRM_OWNR_SYS_ID ";

            var parms = new List<OracleParameter>() { };
            if (criteria.ITEM_CODE != null) parms.Add(new OracleParameter("pITEM_CODE", criteria.ITEM_CODE));
            if (criteria.IPRH_CODE != null) parms.Add(new OracleParameter("pIPRH_CODE", criteria.IPRH_CODE));
            if (criteria.IPRH_DATE != null) parms.Add(new OracleParameter("pIPRH_DATE", SerializeEntity.ConvertDateToDbDate(criteria.IPRH_DATE)));

            if (criteria.IPRD_ITEM_SYS_ID != null) parms.Add(new OracleParameter("pIPRD_ITEM_SYS_ID", criteria.IPRD_ITEM_SYS_ID));
            if (criteria.ITEM_BASIC_SUPP_SYS_ID != null) parms.Add(new OracleParameter("pITEM_BASIC_SUPP_SYS_ID", criteria.ITEM_BASIC_SUPP_SYS_ID));
            if (criteria.IPRD_CPM_FRM_OWNR_SYS_ID != null) parms.Add(new OracleParameter("pIPRD_CPM_FRM_OWNR_SYS_ID", criteria.IPRD_CPM_FRM_OWNR_SYS_ID));

            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetPurchaseBasicQty(PurchaseRequestDetails criteria, string authParms)
        {
            var query = $"SELECT fn_get_ITEM_BASIC_UOM_QTY (:pITEM, :pUOM, :pQTY) AS ITEM_BASIC_QTY FROM DUAL";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pITEM", criteria.IPRD_ITEM_SYS_ID),
                new OracleParameter("pUOM", criteria.IPRD_UOM_SYS_ID),
                new OracleParameter("pQTY", criteria.IPRD_QTY)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
    }
}
