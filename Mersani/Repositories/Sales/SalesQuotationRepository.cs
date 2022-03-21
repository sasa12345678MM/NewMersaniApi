using Mersani.Interfaces.Sales;
using Mersani.models.Auth;
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
    public class SalesQuotationRepository : ISalesQuotationRepo
    {
        public async Task<DataSet> GetSalesquotationHdr(IsalesquotationMaster entity, string PostedType, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $" SELECT CASE" +
                $"         WHEN SQH.SQH_TO_OWNER_CUST_O_C = 'O' THEN ownr.OWNER_NAME_AR" +
                $"          ELSE cust.CUST_NAME_AR" +
                $"       END" +
                $"          AS CustOrOwnerNameAr," +
                $"       CASE" +
                $"          WHEN SQH.SQH_TO_OWNER_CUST_O_C = 'O' THEN ownr.OWNER_NAME_EN" +
                $"          ELSE cust.CUST_NAME_EN" +
                $"       END" +
                $"          AS CustOrOwnerNameEn," +
                $"       SQH.*" +
                $"  FROM SALES_QUOTATION_HDR SQH" +
                $"       LEFT OUTER JOIN GAS_OWNER ownr" +
                $"          ON     SQH.SQH_OWNER_CUST_SYS_ID = ownr.OWNER_SYS_ID" +
                $"             AND SQH.SQH_TO_OWNER_CUST_O_C = 'O'" +
                $"       LEFT OUTER JOIN FINS_CUSTOMER cust" +
                $"          ON     cust.CUST_SYS_ID = SQH.SQH_OWNER_CUST_SYS_ID" +
                $"             AND SQH.SQH_TO_OWNER_CUST_O_C = 'C'" +
                $"where SQH. SQH_SYS_ID = :PSQH_SYS_ID or :PSQH_SYS_ID=0" +
                $" and SQH. SQH_V_CODE ='{auth.User_Act_PH}' ";
            //if (PostedType.Length > 0) { query += " AND( SRIH.RIH_POSTED_Y_N in('" + PostedType + "') or '" + PostedType + "'='ALL' )"; }
            query += $"order by SQH.SQH_SYS_ID DESC";

            var parms = new List<OracleParameter>() {
                new OracleParameter("PSQH_SYS_ID", entity.SQH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetSalesquotationDetails(IsalesquotationDetails entity, string authParms)
        {

            var query = $" select * from SALES_QUOTATION_DTL where SALES_QUOTATION_DTL.SQD_SQH_SYS_ID = :PSQD_SQH_SYS_ID ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PSQD_SQH_SYS_ID", entity.SQD_SQH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);

        }
        public async Task<DataSet> GetSalesquotationTerms(IsalesquotationTerms entity, string authParms)
        {
            var query = $"select * from SALES_QUOTATION_TERMS where SALES_QUOTATION_TERMS.SQT_SQH_SYS_ID = :PSQT_SQH_SYS_ID ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PSQT_SQH_SYS_ID", entity.SQT_SQH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
  

        public async Task<DataSet> GetSalesquotationLastCode(string authParms)
        {
            var query = $"SELECT NVL(MAX (TO_NUMBER (SALES_QUOTATION_HDR.SQH_V_CODE)), 0) + 1 AS Code FROM SALES_QUOTATION_HDR";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }


        public async Task<DataSet> SalesquotationPosting(List<IsalesquotationMaster> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            foreach (IsalesquotationMaster entity in entities)
            {
                entity.CURR_USER = authP.UserCode.Value;

            }
            parameters.Add("xml_document_Mstr", entities.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRCPOSTING_XML", parameters, authParms);
        }

        public async Task<DataSet> SaveInvoicesHdrandDetails(ISalesQuotation entities, string authParms)
        {
            //MSTR
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            //entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
            entities.SALESQUOTATIONMASTER.CURR_USER = authP.UserCode.Value;
            entities.SALESQUOTATIONMASTER.SQH_V_CODE = authP.User_Act_PH;

            if (entities.SALESQUOTATIONMASTER.SQH_SYS_ID > 0)
                entities.SALESQUOTATIONMASTER.STATE = (int)OperationType.Update;
            else entities.SALESQUOTATIONMASTER.STATE = (int)OperationType.Add;
            // DTL
            for (int i = 0; i < entities.SALESQUOTATIONDETAILES.Count; i++)
            {
                entities.SALESQUOTATIONDETAILES[i].CURR_USER = authP.UserCode;
                if (entities.SALESQUOTATIONDETAILES[i].SQD_SYS_ID > 0)
                    if (entities.SALESQUOTATIONDETAILES[i].STATE == 3)
                    {
                        entities.SALESQUOTATIONDETAILES[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.SALESQUOTATIONDETAILES[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.SALESQUOTATIONDETAILES[i].STATE = (int)OperationType.Add;
            }
            // Terms
            for (int i = 0; i < entities.SALESQUOTATIONTERMS.Count; i++)
            {
                entities.SALESQUOTATIONTERMS[i].CURR_USER = authP.UserCode;
                if (entities.SALESQUOTATIONTERMS[i].SQT_SYS_ID > 0)
                    if (entities.SALESQUOTATIONTERMS[i].STATE == 3)
                    {
                        entities.SALESQUOTATIONTERMS[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.SALESQUOTATIONTERMS[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.SALESQUOTATIONTERMS[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.SALESQUOTATIONMASTER });
            parameters.Add("xml_document_d", entities.SALESQUOTATIONDETAILES.ToList<dynamic>());
            parameters.Add("xml_document_T", entities.SALESQUOTATIONTERMS.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_SALES_QUOTATION_XML", parameters, authParms);
        }
        public async Task<DataSet> DeleteSalesquotation(IsalesquotationMaster entity, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entity.CURR_USER = authP.UserCode.Value;
            entity.STATE = (int)OperationType.Delete;

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entity });
            parameters.Add("xml_document_d", new List<dynamic>() { });
            parameters.Add("xml_document_T", new List<dynamic>() { });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_SALES_QUOTATION_XML", parameters, authParms);
        }
    }
}

