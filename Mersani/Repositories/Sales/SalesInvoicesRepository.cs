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
    public class SalesInvoicesRepository : ISalesInvoicesRepo
    {
        public async Task<DataSet> GetInvoicesMaster(SalesInvoices entity, string authParms)
        {
            var query = $"SELECT INV.*, cust.CUST_NAME_AR AS invh_cust_name_ar, cust.CUST_NAME_EN invh_cust_name_en, ACNT.ACC_NO AS INVSH_DR_ACC_NO " +
                $" FROM S_INVOICE_HEAD inv " +
                $" JOIN FINS_ACCOUNT ACNT ON ACNT.ACC_CODE = INV.INVSH_DR_ACC_CODE " +
                $" JOIN FINS_CUSTOMER cust ON cust.CUST_SYS_ID = inv.INVSH_CUST_SYS_ID WHERE (INVSH_SYS_ID = :pINVSH_SYS_ID OR :pINVSH_SYS_ID = 0) AND INVSH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pINVSH_SYS_ID", entity.INVSH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetInvoicesDetails(SalesInvoices entity, string authParms)
        {
            var query = $"SELECT * FROM S_INVOICE_ITEM WHERE INVSI_INVSH_SYS_ID = :pINVSI_INVSH_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pINVSI_INVSH_SYS_ID", entity.INVSH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostInvoicesMasterDetails(SalesInvoicesData entities, string authParms)
        {
            var authData = OracleDQ.GetAuthenticatedUserObject(authParms);
            
            //hdr
            entities.INVOICES_HDR.INVSH_V_CODE = authData.User_Act_PH;
            entities.INVOICES_HDR.CURR_USER = authData.UserCode.Value;
            if (entities.INVOICES_HDR.INVSH_SYS_ID > 0) entities.INVOICES_HDR.STATE = (int)OperationType.Update;
            else entities.INVOICES_HDR.STATE = (int)OperationType.Add;

            // dtl 
            for (int i = 0; i < entities.INVOICES_DTL.Count; i++)
            {
                entities.INVOICES_DTL[i].INVSI_INVSH_SYS_ID = entities.INVOICES_HDR.INVSH_SYS_ID;
                entities.INVOICES_DTL[i].CURR_USER = authData.UserCode;
                if (entities.INVOICES_DTL[i].INVSI_SYS_ID > 0) entities.INVOICES_DTL[i].STATE = (int)OperationType.Update;
                else entities.INVOICES_DTL[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.INVOICES_HDR });
            parameters.Add("xml_document_d", entities.INVOICES_DTL.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_S_INVOICE_XML", parameters, authParms);
        }

        public async Task<DataSet> DeleteInvoicesMasterDetails(SalesInvoiceItems entity, int type, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteDeleteProcAsync("PRC_S_INVOICES_DEL", new { code = type == 1 ? entity.INVSI_INVSH_SYS_ID : entity.INVSI_SYS_ID, type = type }, authParms);
        }


        public async Task<DataSet> GetDefaultAccountsForSales(SalesInvoices entity, string authParms)
        {
            var query = $"SELECT FN_GET_WH_ACC_CODE ('SALVAT', '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') AS S_VAT_ACC_CODE, " +
                $"FN_GET_WH_ACC_CODE('SALVAL', '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') AS S_VAL_ACC_CODE, " +
                $"FN_GET_WH_ACC_CODE('SALDISC', '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') AS S_DIS_ACC_CODE, " +
                $"FN_GET_WH_ACC_CODE ('SALEXP', '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') AS S_EXP_ACC_CODE " +
                $" FROM DUAL";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetInvoicesLastCode(string authParms)
        {
            var query = $"SELECT FN_GET_INV_CODE('SINV', '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') AS INVSH_CODE FROM DUAL";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkSalesPostingInvoices(List<SalesInvoices> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                entity.STATE = (int)OperationType.Update;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                entity.INVSH_V_CODE = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_S_POSTING_INVOICES_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> GetNonPostedInvoices(SalesInvoices entity, string authParms)
        {
            var query = $"SELECT INV.*, cust.CUST_NAME_AR AS invh_cust_name_ar, cust.CUST_NAME_EN invh_cust_name_en, ACNT.ACC_NO AS INVSH_DR_ACC_NO " +
                 $" FROM S_INVOICE_HEAD inv " +
                 $" JOIN FINS_ACCOUNT ACNT ON ACNT.ACC_CODE = INV.INVSH_DR_ACC_CODE " +
                 $" JOIN FINS_CUSTOMER cust ON cust.CUST_SYS_ID = inv.INVSH_CUST_SYS_ID " +
                 $" WHERE (INVSH_SYS_ID = :pINVSH_SYS_ID OR nvl(:pINVSH_SYS_ID,0) = 0) AND INVSH_POSTED_Y_N = 'N' " +
                 $" AND INVSH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pINVSH_SYS_ID", entity.INVSH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
    }
}
