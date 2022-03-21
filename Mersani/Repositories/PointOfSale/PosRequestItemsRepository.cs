using Mersani.Interfaces.PointOfSale;
using Mersani.models.PointOfSale;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.PointOfSale
{
    public class PosRequestItemsRepository : IPosRequestItemsRepo
    {
        public async Task<DataSet> GetPosRequestItemsMaster(PosRequestItemsMaster entity, string authParms)
        {
            var query = $"SELECT MST.*, PH.PHARM_NAME_AR,PH.PHARM_NAME_EN FROM POS_RQST_ITMS_HDR MST, GAS_PHARMACY PH " +
                $" WHERE MST.PRIH_SNDR_PHRM_SYS_ID = PH.PHARM_SYS_ID AND (MST.PRIH_SYS_ID = :pSYS_ID or :pSYS_ID = 0) " +
                $" AND MST.PRIH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", entity.PRIH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetPosRequestItemsMasterForPending(PosRequestItemsMaster entity, string authParms)
        {
            var query = $"SELECT MST.*, PHS.PHARM_NAME_AR AS sndr_ph_ar, PHS.PHARM_NAME_EN sndr_ph_en, PHR.PHARM_NAME_AR AS RQSTR_ph_ar, PHR.PHARM_NAME_EN RQSTR_ph_en " +
                $" FROM POS_RQST_ITMS_HDR MST, GAS_PHARMACY PHS, GAS_PHARMACY PHR " +
                $" WHERE MST.PRIH_SNDR_PHRM_SYS_ID = PHS.PHARM_SYS_ID AND MST.PRIH_RQSTR_PHRM_SYS_ID = PHR.PHARM_SYS_ID AND (MST.PRIH_SYS_ID = :pSYS_ID or :pSYS_ID = 0)";
            var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", entity.PRIH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }


        public async Task<DataSet> GetPosRequestItemsDetails(PosRequestItemsMaster entity, string authParms)
        {
            var query = $"SELECT DTL.*, ITEM.ITEM_NAME_AR, ITEM.ITEM_NAME_EN, UNT.UOM_NAME_AR, UNT.UOM_NAME_EN FROM POS_RQST_ITMS_DTL DTL, INV_ITEM_MASTER ITEM, INV_UOM UNT " +
                $" WHERE DTL.PRID_ITEM_SYS_ID = ITEM.ITEM_SYS_ID AND DTL.PRID_UOM_SYS_ID = UNT.UOM_SYS_ID AND DTL.PRID_PRIH_SYS_ID = :pPRID_PRIH_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pPRID_PRIH_SYS_ID", entity.PRIH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetPosRequestItemsLastCode(string authParms)
        {
            var query = $"SELECT NVL (MAX (TO_NUMBER (PRIH_CODE)), 0) + 1 AS CODE FROM POS_RQST_ITMS_HDR WHERE PRIH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostPosRequestItemsMasterDetails(PosRequestItems entity, string authParms)
        {
            var authData = OracleDQ.GetAuthenticatedUserObject(authParms);

            //hdr
            entity.MASTER.PRIH_V_CODE = authData.User_Act_PH;
            entity.MASTER.CURR_USER = authData.UserCode.Value;
            if (entity.MASTER.PRIH_SYS_ID > 0) entity.MASTER.STATE = (int)OperationType.Update;
            else entity.MASTER.STATE = (int)OperationType.Add;

            // dtl 
            for (int i = 0; i < entity.DETAILS.Count; i++)
            {
                entity.DETAILS[i].PRID_PRIH_SYS_ID = entity.MASTER.PRIH_SYS_ID;
                entity.DETAILS[i].CURR_USER = authData.UserCode;
                if (entity.DETAILS[i].PRID_SYS_ID > 0) entity.DETAILS[i].STATE = (int)OperationType.Update;
                else entity.DETAILS[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entity.MASTER });
            parameters.Add("xml_document_d", entity.DETAILS.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_POS_RQST_ITMS_XML", parameters, authParms);
        }
        public async Task<DataSet> DeletePosRequestItemsMasterDetails(PosRequestItemsDetails entity, int type, string authParms)
        {
            var mstr = new PosRequestItemsMaster();
            var dtls = new List<PosRequestItemsDetails>();
            if (type == 1) {
                mstr.PRIH_SYS_ID = entity.PRID_PRIH_SYS_ID;
                mstr.STATE = 3;
            }
            else dtls.Add(new PosRequestItemsDetails() { STATE = 3, PRID_SYS_ID = entity.PRID_SYS_ID });

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { mstr });
            parameters.Add("xml_document_d", dtls.ToList<dynamic>());

            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_POS_RQST_ITMS_XML", parameters, authParms);
        }

        public async Task<DataSet> GetPosRequestItemsPendingForApproval(PosRequestItemsMaster entity, string authParms)
        {
            var query = $"SELECT MST.*, PH.PHARM_NAME_AR, PH.PHARM_NAME_EN FROM POS_RQST_ITMS_HDR MST, GAS_PHARMACY PH " +
                $" WHERE MST.PRIH_RQSTR_PHRM_SYS_ID = PH.PHARM_SYS_ID AND MST.PRIH_SNDR_PHRM_SYS_ID = :pSYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", entity.PRIH_SNDR_PHRM_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetPosRequestItemsPendigForConfirm(PosRequestItemsMaster entity, string authParms)
        {
            var query = $"SELECT MST.*, PH.PHARM_NAME_AR, PH.PHARM_NAME_EN FROM POS_RQST_ITMS_HDR MST, GAS_PHARMACY PH " +
                $" WHERE MST.PRIH_SNDR_PHRM_SYS_ID = PH.PHARM_SYS_ID AND MST.PRIH_RQSTR_PHRM_SYS_ID = :pSYS_ID AND MST.PRIH_SNDR_APPRVD_Y_N = 'Y'";
            var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", entity.PRIH_RQSTR_PHRM_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
    }
}
