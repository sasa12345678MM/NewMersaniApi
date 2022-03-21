using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mersani.models.Stock;
using Mersani.Interfaces.Stock;
using System.Data;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;

namespace Mersani.Repositories.Stock
{
    public class TransferRequestRepository : ITransferRequestRepo
    {
        public async Task<DataSet> GetTransferRequestMaster(TransferRequestMaster entity, string authParms)
        {
            var query = $"SELECT * FROM (SELECT rqst.*, rqstr.IIM_NAME_AR AS Stock_To_Ar, rqstr.IIM_NAME_EN AS Stock_To_En, rqstd.IIM_NAME_AR AS Stock_From_Ar, rqstd.IIM_NAME_EN AS Stock_From_En, " +
                $" ownr.OWNER_NAME_AR, ownr.OWNER_NAME_EN, rqstr.IIM_V_CODE rqstr_v_code, rqstd.IIM_V_CODE rqstd_v_code " +
                $" FROM INV_TRNSR_REQST_HDR  rqst, INV_INVENTORY_MASTER rqstr, INV_INVENTORY_MASTER rqstd, GAS_OWNER ownr " +
                $" WHERE rqst.ITRH_RQSTR_INV_SYS_ID = rqstr.IIM_SYS_ID AND rqst.ITRH_RQSTd_INV_SYS_ID = rqstd.IIM_SYS_ID AND ownr.OWNER_SYS_ID = rqst.ITRH_OWNER_SYS_ID " +
                $" AND rqstr.IIM_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}' " +
                $" UNION ALL " +
                $" SELECT rqst.*, rqstr.IIM_NAME_AR AS Stock_To_Ar, rqstr.IIM_NAME_EN AS Stock_To_En, rqstd.IIM_NAME_AR AS Stock_From_Ar, rqstd.IIM_NAME_EN AS Stock_From_En, " +
                $" ownr.OWNER_NAME_AR, ownr.OWNER_NAME_EN, rqstr.IIM_V_CODE rqstr_v_code, rqstd.IIM_V_CODE rqstd_v_code " +
                $" FROM INV_TRNSR_REQST_HDR  rqst, INV_INVENTORY_MASTER rqstr, INV_INVENTORY_MASTER rqstd, GAS_OWNER ownr " +
                $" WHERE rqst.ITRH_RQSTR_INV_SYS_ID = rqstr.IIM_SYS_ID AND rqst.ITRH_RQSTd_INV_SYS_ID = rqstd.IIM_SYS_ID AND ownr.OWNER_SYS_ID = rqst.ITRH_OWNER_SYS_ID " +
                $" AND rqstd.IIM_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}' AND rqst.ITRH_APPROVED_Y_N = 'Y') RQSTS " +
                $" WHERE (RQSTS.ITRH_SYS_ID = :pSYS_ID OR :pSYS_ID = 0) ";
            var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", entity.ITRH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetTransferRequestDetails(TransferRequestMaster entity, string authParms)
        {
            var query = $"SELECT * FROM INV_TRNSR_REQST_DTL WHERE ITRD_ITRH_SYS_ID = :pITRD_ITRH_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pITRD_ITRH_SYS_ID", entity.ITRH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostTransferRequestMasterDetails(TransferRequest entity, string authParms)
        {
            var authData = OracleDQ.GetAuthenticatedUserObject(authParms);

            //hdr
            entity.MASTER.ITRH_V_CODE = authData.User_Act_PH;
            entity.MASTER.CURR_USER = authData.UserCode.Value;
            if (entity.MASTER.ITRH_SYS_ID > 0) entity.MASTER.STATE = (int)OperationType.Update;
            else entity.MASTER.STATE = (int)OperationType.Add;

            // dtl 
            for (int i = 0; i < entity.DETAILS.Count; i++)
            {
                entity.DETAILS[i].ITRD_ITRH_SYS_ID = entity.MASTER.ITRH_SYS_ID;
                entity.DETAILS[i].CURR_USER = authData.UserCode;
                if (entity.DETAILS[i].ITRD_SYS_ID > 0) entity.DETAILS[i].STATE = (int)OperationType.Update;
                else entity.DETAILS[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entity.MASTER });
            parameters.Add("xml_document_d", entity.DETAILS.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_TRNSR_REQST_XML", parameters, authParms);
        }
        public async Task<DataSet> DeleteTransferRequestMasterDetails(TransferRequestDetails entity, int type, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteDeleteProcAsync("PRC_INV_TRNSR_REQST_DEL", new { code = type == 1 ? entity.ITRD_ITRH_SYS_ID : entity.ITRD_SYS_ID, type = type }, authParms);
        }

        public async Task<DataSet> GetTransferRequestLastCode(string authParms)
        {
            var query = $"SELECT NVL (MAX (TO_NUMBER (ITRH_CODE)), 0) + 1 AS Code FROM INV_TRNSR_REQST_HDR WHERE ITRH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
    }
}
