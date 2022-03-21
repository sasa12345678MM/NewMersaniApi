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
    public class SalesReturnDeleveryNoteRepository : SalesReturnDeleveryNoteRepo
    {
        public async Task<DataSet> GetRtrnDeleveryNoteHdr(InvSalesRtrnDnHdr entity, string PostedType, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $"  select invm.IIM_NAME_AR as InvNameAr, invm.IIM_NAME_EN as InvNameEn , ISRDH.* " +
                $" from INV_SALES_RTRN_DN_HDR ISRDH inner join INV_INVENTORY_MASTER invm on invm.IIM_SYS_ID = ISRDH.ISRDH_INV_SYS_ID" +
                $" where (ISRDH.ISRDH_SYS_ID = :P_SYS_ID or :P_SYS_ID = 0) " +
                $" and ISRDH.ISRDH_V_CODE ='{auth.User_Act_PH}' ";
            if (PostedType.Length > 0) { query += " AND( ISRDH.ISRDH_APPROVED_Y_N in('" + PostedType + "') or '" + PostedType + "'='ALL' )"; }
            query += $"order by ISRDH_SYS_ID DESC";

            var parms = new List<OracleParameter>() {
                new OracleParameter("P_SYS_ID", entity.ISRDH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }




        public async Task<DataSet> GetInvSalesRtrnDnDtl(InvSalesRtrnDnDtl entity, string authParms)
        {
            var query = $" select * from INV_SALES_RTRN_DN_DTL where INV_SALES_RTRN_DN_DTL.ISRDD_ISRDH_SYS_ID= :PISDD_ISRDH_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PISDD_ISRDH_SYS_ID", entity.ISRDD_ISRDH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> RtrnDeleveryNotePosting(List<InvSalesRtrnDnHdr> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            foreach (InvSalesRtrnDnHdr entity in entities)
            {
                entity.CURR_USER = authP.UserCode.Value;
                entity.ISRDH_APPROVED_BY = authP.UserCode.Value;
                entity.ISRDH_V_CODE = authP.User_Act_PH;

            }
            parameters.Add("xml_document_Mstr", entities.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_S_RTRN_DN_APPROVAL_XML", parameters, authParms);
        }

        public async Task<DataSet> SaveRtrnDeleveryNoteHdrandItem(InvSalesReturnDeleveryNote entities, string authParms)
        {
            //MSTR
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            //entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
            entities.INVSALESRTRNDNHDR.CURR_USER = authP.UserCode.Value;
            entities.INVSALESRTRNDNHDR.ISRDH_V_CODE = authP.User_Act_PH;

            if (entities.INVSALESRTRNDNHDR.ISRDH_SYS_ID > 0)
                entities.INVSALESRTRNDNHDR.STATE = (int)OperationType.Update;
            else entities.INVSALESRTRNDNHDR.STATE = (int)OperationType.Add;
            // DTL
            for (int i = 0; i < entities.INVSALESRTRNDNDTL.Count; i++)
            {
                entities.INVSALESRTRNDNDTL[i].CURR_USER = authP.UserCode;
                if (entities.INVSALESRTRNDNDTL[i].ISRDD_SYS_ID > 0)
                    if (entities.INVSALESRTRNDNDTL[i].STATE == 3)
                    {
                        entities.INVSALESRTRNDNDTL[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.INVSALESRTRNDNDTL[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.INVSALESRTRNDNDTL[i].STATE = (int)OperationType.Add;
            }


            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.INVSALESRTRNDNHDR });
            parameters.Add("xml_document_d", entities.INVSALESRTRNDNDTL.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_SALES_RTRN_DN_XML", parameters, authParms);
        }
        public async Task<DataSet> GetRtrnDeleveryNoteLastCode(string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $"SELECT NVL(MAX (TO_NUMBER (CASE WHEN REGEXP_LIKE (ISRDH_CODE, '^[0-9]+') THEN ISRDH_CODE ELSE '0' END)), 0) +1 AS Code " +
                $" FROM INV_SALES_RTRN_DN_HDR where(ISRDH_V_CODE ='"+ authP.User_Act_PH+ "')";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> DeleteRtrnDeleveryNote(InvSalesRtrnDnHdr entity, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entity.CURR_USER = authP.UserCode.Value;
            entity.STATE = (int)OperationType.Delete;

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entity });
            parameters.Add("xml_document_d", new List<dynamic>() { });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_SALES_RTRN_DN_XML", parameters, authParms);
        }
    }
}