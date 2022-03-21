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
    public class PurchaseInvoicesReturnRepository : PurchaseInvoicesReturnRepo
    {

        public async Task<DataSet> GetInvoicesReturnHdr(InvoicesReturnHead entity, string PostedType, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $"SELECT PRIH.*, supp.SUPP_NAME_AR AS RIH_supp_name_ar, supp.SUPP_NAME_EN RIH_supp_name_en, ACNT.ACC_NO AS RIH_CR_ACC_NO " +
                $"               FROM PR_INVOICE_HEAD PRIH" +
                $"                JOIN FINS_ACCOUNT ACNT ON ACNT.ACC_CODE = PRIH.RIH_CR_ACC_CODE" +
                $"                JOIN FINS_SUPPLIER supp ON supp.SUPP_SYS_ID = PRIH.RIH_SUPP_SYS_ID" +
                $" WHERE(RIH_SYS_ID=:pRIH_SYS_ID or :pRIH_SYS_ID = 0 )" +
                $" and RIH_V_CODE ='{auth.User_Act_PH}' ";
            if (PostedType.Length>0) { query += " AND( PRIH.RIH_POSTED_Y_N in('"+ PostedType + "') or '" + PostedType + "'='ALL' )"; }
            query += $"order by RIH_SYS_ID DESC";
            
            var parms = new List<OracleParameter>() {
                new OracleParameter("pRIH_SYS_ID", entity.RIH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetInvoicesReturnItem(InvoicesReturnItem entity, string authParms)
        {
            var query = $"select * from pR_INVOICE_ITEM where pR_INVOICE_ITEM.RII_RIH_SYS_ID=:PRII_RIH_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PRII_RIH_SYS_ID", entity.RII_RIH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetNonPostedInvoicesReturn(InvoicesReturnHead entity, string authParms)
        {
            var query = $"";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pIADD_IADM_SYS_ID", entity.RIH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> InvoicesReturnPosting(List<InvoicesReturnHead> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            foreach (InvoicesReturnHead entity in entities)
            {
                entity.CURR_USER = authP.UserCode.Value;
                entity.RIH_POSTED_BY= authP.UserCode.Value;
                entity.RIH_V_CODE = authP.User_Act_PH;

            }
            parameters.Add("xml_document_Mstr", entities.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_PR_INVOICE_POSTING_XML", parameters, authParms);
        }

        public async Task<DataSet> SaveInvoicesHdrandItem(InvoiceReturnData entities, string authParms)
        {
            //MSTR
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            //entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
            entities.INVOICERETURNHEAD.CURR_USER = authP.UserCode.Value;
            entities.INVOICERETURNHEAD.RIH_V_CODE = authP.User_Act_PH;

            if (entities.INVOICERETURNHEAD.RIH_SYS_ID > 0)
                entities.INVOICERETURNHEAD.STATE = (int)OperationType.Update;
            else entities.INVOICERETURNHEAD.STATE = (int)OperationType.Add;
            // DTL
            for (int i = 0; i < entities.INVOICERETURNITEM.Count; i++)
            {
                entities.INVOICERETURNITEM[i].CURR_USER = authP.UserCode;
                if (entities.INVOICERETURNITEM[i].RII_SYS_ID > 0)
                    if (entities.INVOICERETURNITEM[i].STATE == 3)
                    {
                        entities.INVOICERETURNITEM[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.INVOICERETURNITEM[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.INVOICERETURNITEM[i].STATE = (int)OperationType.Add;
            }


            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.INVOICERETURNHEAD });
            parameters.Add("xml_document_d", entities.INVOICERETURNITEM.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_PR_INVOICE_XML", parameters, authParms);
        }
        public async Task<DataSet> GetInvoicesLastCode(string authParms)
        {
            var query = $"SELECT NVL (MAX (TO_NUMBER (RIH_CODE)), 0) + 1 AS Code FROM PR_INVOICE_HEAD";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> DeleteInvoicesReturn(InvoicesReturnHead entity, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entity.CURR_USER = authP.UserCode.Value;
            entity.STATE = (int)OperationType.Delete;

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entity });
            parameters.Add("xml_document_d", new List<dynamic>() { });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_PR_INVOICE_XML", parameters, authParms);
        }
        public async Task<DataSet> GetDefaultAccountsForPurchase(InvoicesReturnHead entity, string authParms)
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
