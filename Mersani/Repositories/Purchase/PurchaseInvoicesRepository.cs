using Mersani.Interfaces.Purchase;
using Mersani.models.Purchase;
using Mersani.Oracle;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Mersani.Repositories.Purchase
{
    public class PurchaseInvoicesRepository : IPurchaseInvoicesRepo
    {
        public async Task<DataSet> GetInvoicesMasterDataSearch(dynamic entity, string authParms)
        {
            return await OracleDQ.ExcuteSelectizeProcAsync("PRC_P_INVOICE_HEAD_SEARCH", new List<dynamic>() { entity }, authParms, encodeType: 'Q');
        }

        public async Task<DataSet> GetInvoicesMaster(PurchaseInvoices entity, string authParms)
        {
            var query = $"SELECT INV.*, supp.SUPP_NAME_AR AS invh_supp_name_ar, supp.SUPP_NAME_EN invh_supp_name_en, ACNT.ACC_NO AS INVH_CR_ACC_NO, " +
                $" ('أمر شراء رقم ' || IPOH_CODE ||' - بتاريخ '|| IPOH_DATE) AS PO_NAME_AR, ('Purchase Order No ' || IPOH_CODE || ' With date '||' - '|| IPOH_DATE) AS PO_NAME_EN" +
                $" FROM P_INVOICE_HEAD inv " +
                $" LEFT JOIN FINS_ACCOUNT ACNT ON ACNT.ACC_CODE = INV.INVH_CR_ACC_CODE " +
                $" LEFT JOIN FINS_SUPPLIER supp ON supp.SUPP_SYS_ID = inv.INVH_SUPP_SYS_ID " +
                $" LEFT JOIN INV_PRCH_ORDR_HDR ordr ON ordr.IPOH_SYS_ID = inv.INVH_PO_SYS_ID " +
                $"WHERE (INVH_SYS_ID = :pINVH_SYS_ID OR :pINVH_SYS_ID = 0) " +
                $"AND INVH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pINVH_SYS_ID", entity.INVH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetInvoicesDetails(PurchaseInvoices entity, string authParms)
        {
            var query = $"SELECT * FROM P_INVOICE_ITEM WHERE INVI_INVH_SYS_ID = :pINVI_INVH_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pINVI_INVH_SYS_ID", entity.INVH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostInvoicesMasterDetails(PurchaseInvoicesData entities, string authParms)
        {
            var authData = OracleDQ.GetAuthenticatedUserObject(authParms);

            //hdr
            entities.INVOICES_HDR.INVH_V_CODE = authData.User_Act_PH;
            entities.INVOICES_HDR.CURR_USER = authData.UserCode.Value;
            if (entities.INVOICES_HDR.INVH_SYS_ID > 0) entities.INVOICES_HDR.STATE = (int)OperationType.Update;
            else entities.INVOICES_HDR.STATE = (int)OperationType.Add;

            // dtl 
            for (int i = 0; i < entities.INVOICES_DTL.Count; i++)
            {
                entities.INVOICES_DTL[i].INVI_INVH_SYS_ID = entities.INVOICES_HDR.INVH_SYS_ID;
                entities.INVOICES_DTL[i].CURR_USER = authData.UserCode;
                if (entities.INVOICES_DTL[i].INVI_SYS_ID > 0) entities.INVOICES_DTL[i].STATE = (int)OperationType.Update;
                else entities.INVOICES_DTL[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.INVOICES_HDR });
            parameters.Add("xml_document_d", entities.INVOICES_DTL.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_P_INVOICE_HEAD_XML", parameters, authParms);
        }

        public async Task<DataSet> DeleteInvoicesMasterDetails(PurchaseInvoiceItems entity, int type, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteDeleteProcAsync("PRC_P_INVOICES_DEL", new { code = type == 1 ? entity.INVI_INVH_SYS_ID : entity.INVI_SYS_ID, type = type }, authParms);
        }

        public async Task<DataSet> GetDefaultAccountsForPurchase(PurchaseInvoices entity, string authParms)
        {
            var user = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $"SELECT FN_GET_WH_ACC_CODE ('PURVAT', '{user.User_Act_PH}') AS P_VAT_ACC_CODE, " +
                $"FN_GET_WH_ACC_CODE('PURVAL', '{user.User_Act_PH}') AS P_VAL_ACC_CODE, " +
                $"FN_GET_WH_ACC_CODE('PURDISC', '{user.User_Act_PH}') AS P_DIS_ACC_CODE, " +
                $"FN_GET_WH_ACC_CODE ('PUREXP', '{user.User_Act_PH}') AS P_EXP_ACC_CODE " +
                $"FROM DUAL";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetInvoicesLastCode(string authParms)
        {
            var query = $"SELECT FN_GET_INV_CODE('PINV', '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') AS INVH_CODE FROM DUAL";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkPurchasePostingInvoices(List<PurchaseInvoices> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                entity.STATE = (int)OperationType.Update;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                entity.INVH_V_CODE = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_P_POSTING_INVOICES_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> GetNonPostedInvoices(PurchaseInvoices entity, string authParms)
        {
            var query = $"SELECT INV.*, supp.SUPP_NAME_AR AS invh_supp_name_ar, supp.SUPP_NAME_EN invh_supp_name_en, ACNT.ACC_NO AS INVH_CR_ACC_NO " +
                $" FROM P_INVOICE_HEAD inv " +
                $" JOIN FINS_ACCOUNT ACNT ON ACNT.ACC_CODE = INV.INVH_CR_ACC_CODE " +
                $" JOIN FINS_SUPPLIER supp ON supp.SUPP_SYS_ID = inv.INVH_SUPP_SYS_ID " +
                $" WHERE (INVH_SYS_ID = :pINVH_SYS_ID OR nvl(:pINVH_SYS_ID,0) = 0) AND INVH_POSTED_Y_N = 'N' " +
                $" AND INVH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pINVH_SYS_ID", entity.INVH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        
    }
}