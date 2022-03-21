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
    public class InventoryTransferRepository : IInventoryTransferRepo
    {
        public async Task<DataSet> GetTransferMaster(TransferMaster entity, string authParms)
        {
            //var query = $"SELECT trans.*, stockfrom.IIM_NAME_AR as Stock_From_Ar,stockfrom.IIM_NAME_EN as Stock_From_En, stockto.IIM_NAME_AR as Stock_To_Ar, stockto.IIM_NAME_EN as Stock_To_En " +
            //    $" FROM INV_TRANFER_MSTR trans " +
            //    $" join INV_INVENTORY_MASTER stockfrom on stockfrom.IIM_SYS_ID = trans.ITM_FRM_INV_SYS_ID " +
            //    $" join INV_INVENTORY_MASTER stockto on stockto.IIM_SYS_ID = trans.ITM_TO_INV_SYS_ID " +
            //    $" WHERE (trans.ITM_SYS_ID = :pSYS_ID OR :pSYS_ID = 0)";
            var query = $"SELECT trans.*, stockfrom.IIM_NAME_AR AS Stock_From_Ar, stockfrom.IIM_NAME_EN AS Stock_From_En, stockto.IIM_NAME_AR AS Stock_To_Ar, stockto.IIM_NAME_EN AS Stock_To_En, " +
                $"(CASE  WHEN XYZ.ITM_CODE IS NOT NULL THEN('من: ' || stockfrom.IIM_NAME_AR || ' - ' || 'رقم التحويل: ' || XYZ.ITM_CODE) ELSE '' END) AS TRANSFER_NAME_AR," +
                $"(CASE  WHEN XYZ.ITM_CODE IS NOT NULL THEN('From: ' || stockfrom.IIM_NAME_EN || ' - ' || 'Transfer No: ' || XYZ.ITM_CODE) ELSE '' END) AS TRANSFER_NAME_EN, " +
                $"(CASE  WHEN ABC.ITRH_CODE IS NOT NULL THEN('طلب تحويل رقم ' || ABC.ITRH_CODE || ' بتاريخ ' || ' - ' || ABC.ITRH_DATE) ELSE '' END) AS REQ_NAME_AR, " +
                $"(CASE  WHEN ABC.ITRH_CODE IS NOT NULL THEN('Transfer request no. ' || ABC.ITRH_CODE || ' with date ' || ' - ' || ABC.ITRH_DATE) ELSE '' END) AS REQ_NAME_EN " +
                $" FROM INV_TRANFER_MSTR trans " +
                $" JOIN INV_INVENTORY_MASTER stockfrom ON stockfrom.IIM_SYS_ID = trans.ITM_FRM_INV_SYS_ID " +
                $" JOIN INV_INVENTORY_MASTER stockto ON stockto.IIM_SYS_ID = trans.ITM_TO_INV_SYS_ID " +
                $" LEFT JOIN INV_TRANFER_MSTR XYZ ON XYZ.ITM_SYS_ID = trans.ITM_RELATED_SYS_ID " +
                $" LEFT JOIN INV_TRNSR_REQST_HDR ABC ON ABC.ITRH_SYS_ID = trans.ITM_REQUEST_SYS_ID " +
                $" WHERE (trans.ITM_SYS_ID = :pSYS_ID OR :pSYS_ID = 0)";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pSYS_ID", entity.ITM_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetTransferDetails(TransferMaster entity, string authParms)
        {
            var query = $"SELECT * FROM INV_TRANFER_DTLS WHERE ITD_ITM_SYS_ID = :pITM_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pITM_SYS_ID", entity.ITM_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostTransferMasterDetails(InventoryTransfer entities, string authParms)
        {
            var authData = OracleDQ.GetAuthenticatedUserObject(authParms);

            //hdr
            entities.MASTER.ITM_V_CODE = authData.User_Act_PH;
            entities.MASTER.CURR_USER = authData.UserCode.Value;
            if (entities.MASTER.ITM_SYS_ID > 0) entities.MASTER.STATE = (int)OperationType.Update;
            else entities.MASTER.STATE = (int)OperationType.Add;

            // dtl 
            for (int i = 0; i < entities.DETAILS.Count; i++)
            {
                entities.DETAILS[i].ITD_ITM_SYS_ID = entities.MASTER.ITM_SYS_ID;
                entities.DETAILS[i].CURR_USER = authData.UserCode;
                if (entities.DETAILS[i].ITD_SYS_ID > 0) entities.DETAILS[i].STATE = (int)OperationType.Update;
                else entities.DETAILS[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.MASTER });
            parameters.Add("xml_document_d", entities.DETAILS.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_TRANFER_XML", parameters, authParms);
        }

        public async Task<DataSet> DeleteTransferMasterDetails(TransferDetails entity, int type, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteDeleteProcAsync("PRC_INV_TRANFER_DEL", new { code = type == 1 ? entity.ITD_ITM_SYS_ID : entity.ITD_SYS_ID, type = type }, authParms);
        }

        public async Task<DataSet> GetTransferLastCode(int type, string authParms)
        {
            string query = string.Empty;
            if (type == 1)
                query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (ITM_CODE, '^[0-9]+') THEN ITM_CODE ELSE '0' END)), 0) + 1 AS Code FROM INV_TRANFER_MSTR WHERE ITM_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}' AND ITM_TYPE_LTO_LTI = 'LTO'";
            else
                query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (ITM_CODE, '^[0-9]+') THEN ITM_CODE ELSE '0' END)), 0) + 1 AS Code FROM INV_TRANFER_MSTR WHERE ITM_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}' AND ITM_TYPE_LTO_LTI = 'LTI'";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetTransferDetailsByRequest(int code, string authParms)
        {
            var query = $"SELECT * FROM INV_TRNSR_REQST_DTL WHERE ITRD_ITRH_SYS_ID = :pITRD_ITRH_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pITRD_ITRH_SYS_ID", code) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
    }
}
