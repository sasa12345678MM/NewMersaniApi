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
    public class InvDeleveryNotesRepository : InvDeleveryNotesRepo
    {

        public async Task<DataSet> GetinvDeleveryNoteHdr(invDeleveryNoteHdr entity,string PostedType, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $" SELECT IDH.*,INVM.IIM_NAME_AR,INVM.IIM_NAME_EN FROM inv_Dn_Hdr IDH" +
                $"                INNER JOIN INV_INVENTORY_MASTER INVM ON INVM.IIM_SYS_ID = IDH.IDNH_INV_SYS_ID" +
                $"                WHERE (IDH.IDNH_SYS_ID =:PIDNH_SYS_ID OR :PIDNH_SYS_ID = 0) ";
            if (PostedType.Length > 0) { query += " AND(  IDH.IDNH_APPRVD_Y_N  in('" + PostedType + "') or '" + PostedType + "'='ALL' )"; }
            query += $"order by IDH.IDNH_SYS_ID DESC";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PIDNH_SYS_ID", entity.IDNH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetinvDeleveryNoteDtls(invDeleveryNoteDtl entity, string authParms)
        {
            var query = $"select * from INV_DN_DTL where INV_DN_DTL.IDND_IDNH_SYS_ID=:PIDND_IDNH_SYS_ID or :PIDND_IDNH_SYS_ID=0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PIDND_IDNH_SYS_ID", entity.IDND_IDNH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> PostinvDeleveryNoteHdrDtl(DeleveryNotesData entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entities.INVDELEVERYNOTEHDR.INS_USER = authP.UserCode.Value;
            entities.INVDELEVERYNOTEHDR.IDNH_V_CODE = authP.User_Act_PH;
            
            if (entities.INVDELEVERYNOTEHDR.IDNH_SYS_ID > 0)
                entities.INVDELEVERYNOTEHDR.STATE = (int)OperationType.Update;
            else entities.INVDELEVERYNOTEHDR.STATE = (int)OperationType.Add;
            // DTL
            for (int i = 0; i < entities.INVDELEVERYNOTEDTL.Count; i++)
            {
                entities.INVDELEVERYNOTEDTL[i].INS_USER = authP.UserCode;
                
                if (entities.INVDELEVERYNOTEDTL[i].IDND_IDNH_SYS_ID > 0)
                    if (entities.INVDELEVERYNOTEDTL[i].STATE == 3)
                    {
                        entities.INVDELEVERYNOTEDTL[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.INVDELEVERYNOTEDTL[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.INVDELEVERYNOTEDTL[i].STATE = (int)OperationType.Add;
            }


            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("inv_Dn_Hdr_xml", new List<dynamic>() { entities.INVDELEVERYNOTEHDR });
            parameters.Add("inv_Dn_Dtl_xml", entities.INVDELEVERYNOTEDTL.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_DN_MSTR_DTL_XML", parameters, authParms);
        }
        public async Task<DataSet> DeleteinvDeleveryNoteHdrDtl(invDeleveryNoteHdr entity, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entity.INS_USER = authP.UserCode.Value;
            entity.STATE = (int)OperationType.Delete;
            
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_Hdr", new List<dynamic>() { entity });
            parameters.Add("xml_document_Dtl", new List<dynamic>() {  });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_DN_MSTR_DTL_XML", parameters, authParms);
        }

        public async Task<DataSet> approvalinvDeleveryNote(List<invDeleveryNoteHdr> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            foreach (invDeleveryNoteHdr entity in entities)
            {
                entity.INS_USER = authP.UserCode.Value;
                entity.IDNH_V_CODE = authP.User_Act_PH;

            }
            parameters.Add("inv_Dn_Hdr_xml",  entities.ToList<dynamic>() );
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_APPROV_INV_DN_XML", parameters, authParms);
            //return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_POST_DEL_VOUCHER_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> GetLastCode(int inventory, string type,string authParms)
        {

            var query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (IDNH_CODE, '^[0-9]+') THEN IDNH_CODE ELSE '0' END)), 0) + 1 AS Code " +
                $"FROM INV_DN_HDR where(IDNH_INV_SYS_ID ="+ inventory + " and INVH_TYPE_DNI_DNX_DNS = '"+ type + "')";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
    }
}
