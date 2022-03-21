using Mersani.Interfaces.Stock;
using Mersani.models.Stock;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Stock
{
    public class InvRtrnDeleveryNotesRepository : InvRtrnDeleveryNotesRepo
    {


        public async Task<DataSet> GetRtrnDeleveryNoteHdr(InvRtrnDnHdr entity, string PostedType, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject( authParms);
            var query = $"  SELECT IRDNH.*, INVM.IIM_NAME_AR, INVM.IIM_NAME_EN" +
                $"    FROM inv_RTRN_Dn_Hdr IRDNH" +
                $"         INNER JOIN INV_INVENTORY_MASTER INVM" +
                $"            ON INVM.IIM_SYS_ID = IRDNH.IRDNH_INV_SYS_ID" +
                $"   WHERE(IRDNH.IRDNH_SYS_ID = :PIRDNH_SYS_ID OR: PIRDNH_SYS_ID = 0) ";
            if (PostedType.Length > 0) { query += " AND(  IRDNH.IRDNH_APPRVD_Y_N  in('" + PostedType + "') or '" + PostedType + "'='ALL' )"; }
            query += $"order by IRDNH.IRDNH_SYS_ID DESC";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PIRDNH_SYS_ID", entity.IRDNH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }


        public async Task<DataSet> GetInvRtrnDnDtl(InvRtrnDnDtl entity, string authParms)
        {
            var query = $"select * from INV_RTRN_DN_DTL where INV_RTRN_DN_DTL.IRDND_IRDNH_SYS_ID=:PIRDND_IRDNH_SYS_ID or :PIRDND_IRDNH_SYS_ID=0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PIRDND_IRDNH_SYS_ID", entity.IRDND_IRDNH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> RtrnDeleveryNotePosting(List<InvRtrnDnHdr> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            foreach (InvRtrnDnHdr entity in entities)
            {
                entity.CURR_USER = authP.UserCode.Value;
                entity.IRDNH_APPRVD_BY = authP.UserCode.Value;
                //entity.IRDNH_V_CODE = authP.User_Act_PH;

            }
            parameters.Add("xml_document_Mstr", entities.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_RTRN_DN_APPROVAL_XML", parameters, authParms);
        }

        public async Task<DataSet> SaveRtrnDeleveryNoteHdrandItem(InvRtrnDeleveryNotesData entities, string authParms)
        {
            //MSTR
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            //entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
            entities.INVRTRNDELEVERYNOTEHDR.CURR_USER = authP.UserCode.Value;
            //entities.INVRTRNDELEVERYNOTEHDR.IRDNH_V_CODE = authP.User_Act_PH;

            if (entities.INVRTRNDELEVERYNOTEHDR.IRDNH_SYS_ID > 0)
                entities.INVRTRNDELEVERYNOTEHDR.STATE = (int)OperationType.Update;
            else entities.INVRTRNDELEVERYNOTEHDR.STATE = (int)OperationType.Add;
            // DTL
            for (int i = 0; i < entities.INVRTRNDELEVERYNOTEDTL.Count; i++)
            {
                entities.INVRTRNDELEVERYNOTEDTL[i].CURR_USER = authP.UserCode;
                if (entities.INVRTRNDELEVERYNOTEDTL[i].IRDND_SYS_ID > 0)
                    if (entities.INVRTRNDELEVERYNOTEDTL[i].STATE == 3)
                    {
                        entities.INVRTRNDELEVERYNOTEDTL[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.INVRTRNDELEVERYNOTEDTL[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.INVRTRNDELEVERYNOTEDTL[i].STATE = (int)OperationType.Add;
            }


            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.INVRTRNDELEVERYNOTEHDR });
            parameters.Add("xml_document_d", entities.INVRTRNDELEVERYNOTEDTL.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_RTRN_DN_XML", parameters, authParms);
        }
        public async Task<DataSet> GetRtrnDeleveryNoteLastCode(int inventory, string authParms)
        {
            var query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (IRDNH_CODE, '^[0-9]+') THEN IRDNH_CODE ELSE '0' END)), 0) + 1 AS Code " +
                $"FROM INV_RTRN_DN_HDR where(IRDNH_INV_SYS_ID ="+ inventory + ")";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> DeleteRtrnDeleveryNote(InvRtrnDnHdr entity, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entity.CURR_USER = authP.UserCode.Value;
            entity.STATE = (int)OperationType.Delete;

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entity });
            parameters.Add("xml_document_d", new List<dynamic>() { });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_RTRN_DN_XML", parameters, authParms);
        }
 
    }
}