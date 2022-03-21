using Mersani.Interfaces.Purchase;
using Mersani.models.Purchase;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Purchase
{
    public class InvPrchReturnOrdrRepository : InvPrchReturnOrdrRepo
    {
        public async Task<DataSet> GetInvPrchReturnHdr(InvPrchReturnOrdrHdr entity, string PostedType, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $"SELECT * FROM INV_PRCH_RETURN_ORDR_HDR where (IPROH_SYS_ID=:SysId or :SysId=0)" +
                $" and IPROH_V_CODE ='{auth.User_Act_PH}' ";
            if (PostedType.Length > 0) { query += " AND( IPROH_APPRVD_Y_N in('" + PostedType + "') or '" + PostedType + "'='ALL' )"; }
            query += $"order by IPROH_SYS_ID DESC";

            var parms = new List<OracleParameter>() {
                new OracleParameter("SysId", entity.IPROH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetInvPrchReturnOrdrDtl(InvPrchReturnOrdrDtl entity, string authParms)
        {
            var query = $"SELECT* FROM INV_PRCH_RETURN_ORDR_Dtl where IPROD_IPROH_SYS_ID =:ParentSysId";
            var parms = new List<OracleParameter>() {
                new OracleParameter("ParentSysId", entity.IPROD_IPROH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetNonPostedInvPrchReturn(InvPrchReturnOrdrHdr entity, string authParms)
        {
            var query = $"SELECT* FROM INV_PRCH_RETURN_ORDR_HDR ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("ParentSysId", entity.IPROH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> InvPrchReturnPosting(List<InvPrchReturnOrdrHdr> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            foreach (InvPrchReturnOrdrHdr entity in entities)
            {
                entity.CURR_USER = authP.UserCode.Value;
                entity.IPROH_APPRVD_BY = authP.UserCode.Value;
                entity.IPROH_V_CODE = authP.User_Act_PH;

            }
            parameters.Add("xml_document_Mstr", entities.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("INV_PRCH_RET_ORDR_APPRVD_XML", parameters, authParms);
        }

        public async Task<DataSet> SaveInvPrchHdrandItem(PurchaseReturnOrderData entities, string authParms)
        {
            //MSTR
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            //entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
            entities.PURCHASERETURNORDERHDR.CURR_USER = authP.UserCode.Value;
            entities.PURCHASERETURNORDERHDR.IPROH_V_CODE = authP.User_Act_PH;

            if (entities.PURCHASERETURNORDERHDR.IPROH_SYS_ID > 0)
                entities.PURCHASERETURNORDERHDR.STATE = (int)OperationType.Update;
            else entities.PURCHASERETURNORDERHDR.STATE = (int)OperationType.Add;
            // DTL
            for (int i = 0; i < entities.PURCHASERETURNORDERDTL.Count; i++)
            {
                entities.PURCHASERETURNORDERDTL[i].CURR_USER = authP.UserCode;
                entities.PURCHASERETURNORDERDTL[i].IPROD_IPROH_SYS_ID = entities.PURCHASERETURNORDERHDR.IPROH_SYS_ID;
                if (entities.PURCHASERETURNORDERDTL[i].IPROD_SYS_ID > 0)
                    if (entities.PURCHASERETURNORDERDTL[i].STATE == 3)
                    {
                        entities.PURCHASERETURNORDERDTL[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.PURCHASERETURNORDERDTL[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.PURCHASERETURNORDERDTL[i].STATE = (int)OperationType.Add;
            }


            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.PURCHASERETURNORDERHDR });
            parameters.Add("xml_document_d", entities.PURCHASERETURNORDERDTL.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("INV_PRCH_RETURN_ORDR_XML", parameters, authParms);
        }
        public async Task<DataSet> GetInvPrchLastCode(string authParms)
        {
            var query = "SELECT NVL (MAX (TO_NUMBER (IPROH_CODE)), 0) + 1 AS Code FROM INV_PRCH_RETURN_ORDR_HDR"; //$"SELECT NVL (MAX (TO_NUMBER (RIH_SYS_ID)), 0) + 1 AS Code FROM PR_INVOICE_HEAD";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> DeleteInvPrchReturn(InvPrchReturnOrdrHdr entity, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entity.CURR_USER = authP.UserCode.Value;
            entity.STATE = (int)OperationType.Delete;

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entity });
            parameters.Add("xml_document_d", new List<dynamic>() { });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("INV_PRCH_RETURN_ORDR_XML", parameters, authParms);
        }
        public async Task<DataSet> GetDefaultAccountsForPurchase(InvPrchReturnOrdrHdr entity, string authParms)
        {
            var user = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $"SELECT FN_GET_WH_ACC_CODE ('PURVAT', '{user.User_Act_PH}') AS P_VAT_ACC_CODE, " +
                $"FN_GET_WH_ACC_CODE('PURVAL', '{user.User_Act_PH}') AS P_VAL_ACC_CODE, " +
                $"FN_GET_WH_ACC_CODE('PURDISC', '{user.User_Act_PH}') AS P_DIS_ACC_CODE, " +
                $"FN_GET_WH_ACC_CODE ('PUREXP', '{user.User_Act_PH}') AS P_EXP_ACC_CODE " +
                $"FROM DUAL";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
    }
    }

