using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using Mersani.Interfaces.Sales;
using System.Threading.Tasks;
using Mersani.models.Sales;
using Mersani.Oracle;
using System.Data;
using System.Linq;

namespace Mersani.Repositories.Sales
{
    public class SalesPaymentRepository : ISalesPaymentRepo
    {
        public async Task<DataSet> GetSalesPaymentMaster(S_PaymentMaster entity, string authParms)
        {
            var query = $"SELECT pay.*, cust.CUST_NAME_AR AS pay_cust_name_ar, cust.CUST_NAME_EN AS pay_cust_name_en " +
                $" FROM S_PAYMENT_MST pay JOIN FINS_CUSTOMER cust ON cust.CUST_SYS_ID = pay.S_PAY_CUST_SYS_ID " +
                $" WHERE (pay.S_PAY_SYS_ID = :pSYS_ID OR :pSYS_ID = 0) AND pay.S_PAY_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pSYS_ID", entity.S_PAY_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetSalesPaymentDetails(S_PaymentMaster entity, string authParms)
        {
            string query;
            var parms = new List<OracleParameter>();
            if (entity.S_PAY_SYS_ID > 0)
            {
                query = $"SELECT S_PAYMENT_DTLS.*, S_INVOICE_HEAD.INVSH_CODE AS S_PAY_INV_NO " +
                    $" FROM S_PAYMENT_DTLS JOIN S_INVOICE_HEAD ON S_PAYMENT_DTLS.S_PAY_DTLS_INV_SYS_ID = S_INVOICE_HEAD.INVSH_SYS_ID " +
                    $" WHERE S_PAY_MST_SYS_ID = :pS_PAY_MST_SYS_ID AND S_INVOICE_HEAD.INVSH_POSTED_Y_N = 'Y'";
                parms.Add(new OracleParameter("pS_PAY_MST_SYS_ID", entity.S_PAY_SYS_ID));
            }
            else
            {
                query = $"SELECT * FROM SALES_INV_PAYMENT WHERE S_PAY_CUSTOMER_ID = :pCustomer_Id AND S_PAY_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
                parms.Add(new OracleParameter("pCustomer_Id", entity.S_PAY_CUST_SYS_ID));
            }
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostSalesPaymentMasterDetails(SalesPayment entities, string authParms)
        {
            var authData = OracleDQ.GetAuthenticatedUserObject(authParms);

            //hdr
            entities.PAYMENT_HDR.S_PAY_V_CODE = authData.User_Act_PH;
            entities.PAYMENT_HDR.CURR_USER = authData.UserCode.Value;
            if (entities.PAYMENT_HDR.S_PAY_SYS_ID > 0) entities.PAYMENT_HDR.STATE = (int)OperationType.Update;
            else entities.PAYMENT_HDR.STATE = (int)OperationType.Add;

            // dtl 
            for (int i = 0; i < entities.PAYMENT_DTL.Count; i++)
            {
                entities.PAYMENT_DTL[i].S_PAY_MST_SYS_ID = entities.PAYMENT_HDR.S_PAY_SYS_ID;
                entities.PAYMENT_DTL[i].CURR_USER = authData.UserCode;

                entities.PAYMENT_DTL[i].S_PAY_V_CODE = authData.User_Act_PH;
                entities.PAYMENT_DTL[i].S_PAY_POSTED_Y_N = entities.PAYMENT_HDR.S_PAY_POSTED_Y_N;
                entities.PAYMENT_DTL[i].S_PAY_DR_ACC_CODE = entities.PAYMENT_HDR.S_PAY_DR_ACC_CODE;
                entities.PAYMENT_DTL[i].S_PAY_CR_ACC_CODE = entities.PAYMENT_HDR.S_PAY_CR_ACC_CODE;
                entities.PAYMENT_DTL[i].S_PAY_NOTE = entities.PAYMENT_HDR.S_PAY_NOTE;

                if (entities.PAYMENT_DTL[i].S_PAY_DTLS_SYS_ID > 0) entities.PAYMENT_DTL[i].STATE = (int)OperationType.Update;
                else entities.PAYMENT_DTL[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.PAYMENT_HDR });
            parameters.Add("xml_document_d", entities.PAYMENT_DTL.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_S_PAYMENT_XML", parameters, authParms);
        }

        public async Task<DataSet> DeleteSalesPaymentMasterDetails(S_PaymentDetails entity, int type, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteDeleteProcAsync("PRC_S_PAYMENT_DEL", new { code = type == 1 ? entity.S_PAY_MST_SYS_ID : entity.S_PAY_DTLS_SYS_ID, type = type }, authParms);
        }

        public async Task<DataSet> GetSalesInvoicesByCustomer(S_PaymentMaster entity, string authParms)
        {
            var query = $"SELECT INV.*, cust.CUST_NAME_AR AS invh_cust_name_ar, cust.CUST_NAME_EN invh_cust_name_en, CALCULATE_S_INV_TOTAL(INV.INVSH_SYS_ID) AS Total_Price FROM S_INVOICE_HEAD INV" +
                $" JOIN FINS_CUSTOMER cust ON cust.CUST_SYS_ID = inv.INVSH_CUST_SYS_ID WHERE INV.INVSH_CUST_SYS_ID = :pCUST_SYS_ID AND INV.INVSH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pCUST_SYS_ID", entity.S_PAY_CUST_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetPaymentLastCode(string authParms)
        {
            var query = $"SELECT FN_GET_PAYMENT_CODE('SPAY', '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') AS S_PAY_CODE FROM DUAL";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkSalesApprovedPayments(List<S_PaymentMaster> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                entity.STATE = (int)OperationType.Update;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                entity.S_PAY_V_CODE = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_S_PAYMENT_POSTING_XML", entities.ToList<dynamic>(), authParms);
        }
    }
}
