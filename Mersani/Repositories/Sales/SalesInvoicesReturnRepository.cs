using Mersani.Interfaces.Sales;
using Mersani.models.Sales;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Sales
{
    public class SalesInvoicesReturnRepository : SalesInvoicesReturnRepo
    {

        public async Task<DataSet> GetSalesInvoicesReturnHdr(SalesInvoicesReturnHead entity, string PostedType, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $"SELECT SRIH.*, Cust.CUST_NAME_AR AS RIH_CUST_NAME_AR, Cust.CUST_NAME_EN AS RIH_CUST_NAME_EN, ACNT.ACC_NO AS RIH_CR_ACC_NO" +
                $"                               FROM SR_INVOICE_HEAD SRIH" +
                $"                                JOIN FINS_ACCOUNT ACNT ON ACNT.ACC_CODE = SRIH.RIH_CR_ACC_CODE" +
                $"                                JOIN FINS_CUSTOMER Cust ON Cust.CUST_SYS_ID = SRIH.RIH_CUST_SYS_ID" +
                $"                WHERE(RIH_SYS_ID=:SRIH_SYS_ID or :SRIH_SYS_ID = 0 )" +
                $" and RIH_V_CODE ='{auth.User_Act_PH}' ";
            if (PostedType.Length > 0) { query += " AND( SRIH.RIH_POSTED_Y_N in('" + PostedType + "') or '" + PostedType + "'='ALL' )"; }
            query += $"order by RIH_SYS_ID DESC";

            var parms = new List<OracleParameter>() {
                new OracleParameter("SRIH_SYS_ID", entity.RIH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetSalesInvoicesReturnItem(SalesInvoicesReturnItem entity, string authParms)
        {
            var query = $" select * from SR_INVOICE_ITEM where SR_INVOICE_ITEM.RII_RIH_SYS_ID=:PRII_RIH_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PRII_RIH_SYS_ID", entity.RII_RIH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetNonPostedSalesInvoicesReturn(SalesInvoicesReturnHead entity, string authParms)
        {
            var query = $"";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pIADD_IADM_SYS_ID", entity.RIH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> SalesInvoicesReturnPosting(List<SalesInvoicesReturnHead> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            foreach (SalesInvoicesReturnHead entity in entities)
            {
                entity.CURR_USER = authP.UserCode.Value;
                entity.RIH_POSTED_BY = authP.UserCode.Value;
                entity.RIH_V_CODE = authP.User_Act_PH;

            }
            parameters.Add("xml_document_Mstr", entities.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_SR_INVOICE_POSTING_XML", parameters, authParms);
        }

        public async Task<DataSet> SaveInvoicesHdrandItem(SalesInvoiceReturnData entities, string authParms)
        {
            //MSTR
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            //entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
            entities.SALESINVOICERETURNHEAD.CURR_USER = authP.UserCode.Value;
            entities.SALESINVOICERETURNHEAD.RIH_V_CODE = authP.User_Act_PH;

            if (entities.SALESINVOICERETURNHEAD.RIH_SYS_ID > 0)
                entities.SALESINVOICERETURNHEAD.STATE = (int)OperationType.Update;
            else entities.SALESINVOICERETURNHEAD.STATE = (int)OperationType.Add;
            // DTL
            for (int i = 0; i < entities.SALESINVOICERETURNITEM.Count; i++)
            {
                entities.SALESINVOICERETURNITEM[i].CURR_USER = authP.UserCode;
                if (entities.SALESINVOICERETURNITEM[i].RII_SYS_ID > 0)
                    if (entities.SALESINVOICERETURNITEM[i].STATE == 3)
                    {
                        entities.SALESINVOICERETURNITEM[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.SALESINVOICERETURNITEM[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.SALESINVOICERETURNITEM[i].STATE = (int)OperationType.Add;
            }


            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.SALESINVOICERETURNHEAD });
            parameters.Add("xml_document_d", entities.SALESINVOICERETURNITEM.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_SR_INVOICE_XML", parameters, authParms);
        }
        public async Task<DataSet> GetInvoicesLastCode(string authParms)
        {
            var query = $"SELECT NVL (MAX (TO_NUMBER (RIH_CODE)), 0) + 1 AS Code FROM SR_INVOICE_HEAD";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> DeleteSalesInvoicesReturn(SalesInvoicesReturnHead entity, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entity.CURR_USER = authP.UserCode.Value;
            entity.STATE = (int)OperationType.Delete;

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entity });
            parameters.Add("xml_document_d", new List<dynamic>() { });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_SR_INVOICE_XML", parameters, authParms);
        }
        public async Task<DataSet> GetDefaultAccountsForPurchase(SalesInvoicesReturnHead entity, string authParms)
        {
            var user = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $"SELECT FN_GET_WH_ACC_CODE ('SALVAT', '{user.User_Act_PH}') AS S_VAT_ACC_CODE, " +
                     $"FN_GET_WH_ACC_CODE('SALVAL', '{user.User_Act_PH}') AS S_VAL_ACC_CODE, " +
                     $"FN_GET_WH_ACC_CODE('SALDISC', '{user.User_Act_PH}') AS S_DIS_ACC_CODE, " +
                     $"FN_GET_WH_ACC_CODE ('SALEXP', '{user.User_Act_PH}') AS S_EXP_ACC_CODE " +
                     $" FROM DUAL";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }


    }
}
