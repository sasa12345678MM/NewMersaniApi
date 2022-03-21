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
    public class PurchaseOrderRepository : IPurchaseOrderRepo
    {
        public async Task<DataSet> GetPurchaseOrderMaster(PurchaseOrderMaster entity, string authParms)
        {
            var query = $"SELECT ordr.*, supp.SUPP_NAME_AR AS IPOH_SUPP_NAME_AR, supp.SUPP_NAME_EN IPOH_SUPP_NAME_EN, " +
                $" ('طلب مشتريات رقم ' || rqst.IPRH_CODE || ' - بتاريخ  ' || rqst.IPRH_DATE) AS RQST_NAME_AR, ('Purchase Request No ' || rqst.IPRH_CODE || ' With Date ' || ' - ' || rqst.IPRH_DATE) AS RQST_NAME_EN " +
                $" FROM INV_PRCH_ORDR_HDR ordr " +
                $" JOIN FINS_SUPPLIER supp ON supp.SUPP_SYS_ID = ordr.IPOH_SUPP_SYS_ID " +
                $" LEFT JOIN INV_PRCH_REQST_HDR rqst ON rqst.IPRH_SYS_ID = ordr.IPOH_IPRH_SYS_ID " +
                $" WHERE (IPOH_SYS_ID = :pIPOH_SYS_ID OR :pIPOH_SYS_ID = 0) AND IPOH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pIPOH_SYS_ID", entity.IPOH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetPurchaseOrderDetails(PurchaseOrderMaster entity, string authParms)
        {
            var query = $"SELECT * FROM INV_PRCH_ORDR_DTL WHERE IPOD_IPOH_SYS_ID = :pIPOD_IPOH_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pIPOD_IPOH_SYS_ID", entity.IPOH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetOrderDetailsByRequest(int code, string authParms)
        {
            var query = $"SELECT dtl.*, item.ITEM_SUPP_SYS_ID FROM V_INV_PRCH_RQST_DTL_NO_PO dtl JOIN INV_ITEM_MASTER item ON item.ITEM_SYS_ID = dtl.IPRD_ITEM_SYS_ID WHERE IPRD_IPRH_SYS_ID = :pCode";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pCode", code)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetPurchaseOrderLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX (TO_NUMBER (IPOH_CODE)), 0) + 1 AS Code FROM INV_PRCH_ORDR_HDR WHERE IPOH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostPurchaseOrderMasterDetails(PurchaseOrder entities, string authParms)
        {
            var authData = OracleDQ.GetAuthenticatedUserObject(authParms);

            //hdr
            entities.MASTER.IPOH_V_CODE = authData.User_Act_PH;
            entities.MASTER.CURR_USER = authData.UserCode.Value;
            if (entities.MASTER.IPOH_SYS_ID > 0) entities.MASTER.STATE = (int)OperationType.Update;
            else entities.MASTER.STATE = (int)OperationType.Add;

            // dtl 
            for (int i = 0; i < entities.DETAILS.Count; i++)
            {
                entities.DETAILS[i].IPOD_IPOH_SYS_ID = entities.MASTER.IPOH_SYS_ID;
                entities.DETAILS[i].CURR_USER = authData.UserCode;
                if (entities.DETAILS[i].IPOD_SYS_ID > 0) entities.DETAILS[i].STATE = (int)OperationType.Update;
                else entities.DETAILS[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.MASTER });
            parameters.Add("xml_document_d", entities.DETAILS.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_PRCH_ORDR_XML", parameters, authParms);
        }

        public async Task<DataSet> BulkPurchaseApprovedOrders(List<PurchaseOrderMaster> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                entity.STATE = (int)OperationType.Update;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                entity.IPOH_V_CODE = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_PRCH_ORDR_APPROVE_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeletePurchaseOrderMasterDetails(PurchaseOrderDetails entity, int type, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteDeleteProcAsync("PRC_INV_PRCH_ORDR_DEL", new { code = type == 1 ? entity.IPOD_IPOH_SYS_ID : entity.IPOD_SYS_ID, type = type }, authParms);
        }

        public async Task<DataSet> GetNonApprovedOrders(PurchaseOrderMaster entity, string authParms)
        {
            var query = $"SELECT ordr.*, supp.SUPP_NAME_AR AS IPOH_SUPP_NAME_AR, supp.SUPP_NAME_EN IPOH_SUPP_NAME_EN FROM INV_PRCH_ORDR_HDR ordr " +
                $" JOIN FINS_SUPPLIER supp ON supp.SUPP_SYS_ID = ordr.IPOH_SUPP_SYS_ID WHERE (IPOH_SYS_ID = :pIPOH_SYS_ID OR nvl(:pIPOH_SYS_ID,0) = 0) AND IPOH_POSTED_Y_N = 'N' " +
                $" AND IPOH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pIPOH_SYS_ID", entity.IPOH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
    }
}
