using Mersani.Interfaces.Purchase;
using Mersani.models.Purchase;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Purchase
{
    public class ParchaseRequestRepository : IPurchaseRequestRepo
    {
        public async Task<DataSet> GetPurchaseRequestMaster(PurchaseRequestMaster entity, string authParms)
        {
            var query = $"SELECT * FROM INV_PRCH_REQST_HDR where (IPRH_SYS_ID = :pSYS_ID or :pSYS_ID = 0) and IPRH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", entity.IPRH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetPurchaseRequestDetails(PurchaseRequestMaster entity, string authParms)
        {
            var query = $"SELECT * FROM INV_TRNSR_REQST_DTL WHERE ITRD_ITRH_SYS_ID = :pITRD_ITRH_SYS_ID";
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

        public async Task<DataSet> GetPurchaseRequestLastCode(string authParms)
        {
            var query = $"SELECT NVL (MAX (TO_NUMBER (ITRH_CODE)), 0) + 1 AS Code FROM INV_TRNSR_REQST_HDR WHERE ITRH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> DeletePurchaseRequestMasterDetails(PurchaseRequestDetails entity, int type, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteDeleteProcAsync("PRC_INV_PRCH_REQST_DEL", new { code = type == 1 ? entity.IPRD_IPRH_SYS_ID : entity.IPRD_SYS_ID, type = type }, authParms);
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
            var query = $"SELECT * FROM V_INV_PRCH_REQST_HDR " +
                $" WHERE IPRH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'" +
                $" AND (ITEM_CODE = :pITEM_CODE OR :pITEM_CODE IS NULL) " +
                $" AND (IPRH_CODE = :pIPRH_CODE OR :pIPRH_CODE IS NULL) " +
                $" AND (TO_DATE(IPRH_DATE) = TO_DATE(:pIPRH_DATE) OR :pIPRH_DATE IS NULL) ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pITEM_CODE", criteria.ITEM_CODE),
                new OracleParameter("pIPRH_CODE", criteria.IPRH_CODE),
                new OracleParameter("pIPRH_DATE", criteria.IPRH_DATE),
            };
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
