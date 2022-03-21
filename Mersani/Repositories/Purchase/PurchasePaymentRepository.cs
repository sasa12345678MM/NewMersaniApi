using Oracle.ManagedDataAccess.Client;
using Mersani.Interfaces.Purchase;
using System.Collections.Generic;
using Mersani.models.Purchase;
using System.Threading.Tasks;
using Mersani.Oracle;
using System.Data;
using System.Linq;

namespace Mersani.Repositories.Purchase
{
    public class PurchasePaymentRepository : IPurchasePaymentRepo
    {
        public async Task<DataSet> GetPurchasePaymentMaster(P_PaymentMaster entity, string authParms)
        {
            var query = $"SELECT pay.*, supp.SUPP_NAME_AR AS P_PAY_SUPP_NAME_AR, supp.SUPP_NAME_EN AS P_PAY_SUPP_NAME_EN " +
                $" FROM P_PAYMENT_MST pay JOIN FINS_SUPPLIER supp ON supp.SUPP_SYS_ID = pay.P_PAY_SUPP_SYS_ID " +
                $" WHERE (pay.P_PAY_SYS_ID = :pSYS_ID OR :pSYS_ID = 0) AND pay.P_PAY_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pSYS_ID", entity.P_PAY_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetPurchasePaymentDetails(P_PaymentMaster entity, string authParms)
        {
            string query;
            var parms = new List<OracleParameter>();
            if (entity.P_PAY_SYS_ID > 0)
            {
                query = $"SELECT P_PAYMENT_DTLS.*, P_INVOICE_HEAD.INVH_CODE AS P_PAY_INV_NO " +
                    $" FROM P_PAYMENT_DTLS JOIN P_INVOICE_HEAD ON P_PAYMENT_DTLS.P_PAY_DTLS_INV_SYS_ID = P_INVOICE_HEAD.INVH_SYS_ID " +
                    $" WHERE P_PAY_MST_SYS_ID = :pP_PAY_MST_SYS_ID AND P_INVOICE_HEAD.INVH_POSTED_Y_N = 'Y'";
                parms.Add(new OracleParameter("pP_PAY_MST_SYS_ID", entity.P_PAY_SYS_ID));
            }
            else
            {
                query = $"SELECT * FROM PURCHASE_INV_PAYMENT WHERE P_PAY_SUPPLIER_ID = :pSupplier_Id AND P_PAY_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
                parms.Add(new OracleParameter("pSupplier_Id", entity.P_PAY_SUPP_SYS_ID));
            }
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostPurchasePaymentMasterDetails(PurchasePayment entities, string authParms)
        {
            var authData = OracleDQ.GetAuthenticatedUserObject(authParms);

            //hdr
            entities.PAYMENT_HDR.P_PAY_V_CODE = authData.User_Act_PH;
            entities.PAYMENT_HDR.CURR_USER = authData.UserCode.Value;
            if (entities.PAYMENT_HDR.P_PAY_SYS_ID > 0) entities.PAYMENT_HDR.STATE = (int)OperationType.Update;
            else entities.PAYMENT_HDR.STATE = (int)OperationType.Add;

            // dtl 
            for (int i = 0; i < entities.PAYMENT_DTL.Count; i++)
            {
                entities.PAYMENT_DTL[i].P_PAY_MST_SYS_ID = entities.PAYMENT_HDR.P_PAY_SYS_ID;
                entities.PAYMENT_DTL[i].CURR_USER = authData.UserCode;

                entities.PAYMENT_DTL[i].P_PAY_V_CODE = authData.User_Act_PH;
                entities.PAYMENT_DTL[i].P_PAY_POSTED_Y_N = entities.PAYMENT_HDR.P_PAY_POSTED_Y_N;
                entities.PAYMENT_DTL[i].P_PAY_DR_ACC_CODE = entities.PAYMENT_HDR.P_PAY_DR_ACC_CODE;
                entities.PAYMENT_DTL[i].P_PAY_CR_ACC_CODE = entities.PAYMENT_HDR.P_PAY_CR_ACC_CODE;
                entities.PAYMENT_DTL[i].P_PAY_NOTE = entities.PAYMENT_HDR.P_PAY_NOTE;


                if (entities.PAYMENT_DTL[i].P_PAY_DTLS_SYS_ID > 0) entities.PAYMENT_DTL[i].STATE = (int)OperationType.Update;
                else entities.PAYMENT_DTL[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.PAYMENT_HDR });
            parameters.Add("xml_document_d", entities.PAYMENT_DTL.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_P_PAYMENT_XML", parameters, authParms);
        }

        public async Task<DataSet> DeletePurchasePaymentMasterDetails(P_PaymentDetails entity, int type, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteDeleteProcAsync("PRC_P_PAYMENT_DEL", new { code = type == 1 ? entity.P_PAY_MST_SYS_ID : entity.P_PAY_DTLS_SYS_ID, type = type }, authParms);
        }

        public async Task<DataSet> GetPaymentLastCode(string authParms)
        {
            var query = $"SELECT FN_GET_PAYMENT_CODE('PPAY', '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') AS P_PAY_CODE FROM DUAL";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkPurchaseApprovedPayments(List<P_PaymentMaster> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                entity.STATE = (int)OperationType.Update;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                entity.P_PAY_V_CODE = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_P_PAYMENT_POSTING_XML", entities.ToList<dynamic>(), authParms);
        }
    }
}
