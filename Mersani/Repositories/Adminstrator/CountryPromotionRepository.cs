using Mersani.Interfaces.Administrator;
using Mersani.models;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class CountryPromotionRepository : CountryPromotionRepo
    {
        public async  Task<DataSet> DeleteCountryPromotionHdr(COUNTRY_PROMOTION_HDR entity, int v, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entity.CURR_USER = authP.UserCode.Value;
            entity.STATE = (int)OperationType.Delete;
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_Hdr", new List<dynamic>() { entity });
            parameters.Add("xml_document_Dtl", new List<dynamic>() { });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_GASCOUNTRYPROMOTION_XML", parameters, authParms);
        }

        public async  Task<DataSet> GetCountryPromotionDtl(COUNTRY_PROMOTION_DTL entity, string authParms)
        {
            var query = $"  select* from Gas_COUNTRY_PROMOTION_Dtl where GCPD_GCPH_SYS_ID =:PGCPD_GCPH_SYS_ID";

            var parms = new List<OracleParameter>() {
                new OracleParameter("PGCPD_GCPH_SYS_ID", entity.GCPD_GCPH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);

        }

        public async Task<DataSet> GetCountryPromotionHdr(COUNTRY_PROMOTION_HDR entity, string authParms)
        {
            var query = $" select * from Gas_COUNTRY_PROMOTION_HDR where (GCPH_SYS_ID={entity.GCPH_SYS_ID}or {entity.GCPH_SYS_ID}=0 ) " +
                $"and(GCPH_C_SYS_ID ={entity.GCPH_C_SYS_ID} or {entity.GCPH_C_SYS_ID} = 0)";
           
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);

        }


        public async  Task<DataSet> GetLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (TSOH_CODE, '^[0-9]+') THEN TSOH_CODE ELSE '0' END)), 0) + 1 AS Code " +
             $" from TKT_SALES_ORDER_HDR";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostCountryPromotion(COUNTRY_PROMOTION entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            //entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
            entities.GASCOUNTRYPROMOTIONHDR.CURR_USER = authP.UserCode.Value;
            //entities.TKTSALESORDERHDR.TSOH_CURRENT_STATUS_USR= authP.UserCode.Value;
            if (entities.GASCOUNTRYPROMOTIONHDR.GCPH_SYS_ID > 0)
                entities.GASCOUNTRYPROMOTIONHDR.STATE = (int)OperationType.Update;
            else entities.GASCOUNTRYPROMOTIONHDR.STATE = (int)OperationType.Add;
            // DTL
            for (int i = 0; i < entities.GASCOUNTRYPROMOTIONDTL.Count; i++)
            {
                entities.GASCOUNTRYPROMOTIONDTL[i].CURR_USER = authP.UserCode;
                if (entities.GASCOUNTRYPROMOTIONDTL[i].GCPD_SYS_ID > 0)
                    if (entities.GASCOUNTRYPROMOTIONDTL[i].STATE == 3)
                    {
                        entities.GASCOUNTRYPROMOTIONDTL[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.GASCOUNTRYPROMOTIONDTL[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.GASCOUNTRYPROMOTIONDTL[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.GASCOUNTRYPROMOTIONHDR });
            parameters.Add("xml_document_d", entities.GASCOUNTRYPROMOTIONDTL.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_GASCOUNTRYPROMOTION_XML", parameters, authParms);
        }
    }
}
