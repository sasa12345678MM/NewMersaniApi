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
    public class InvAssmblyItemRepository : InvAssmblyItemRepo
    {
        public async Task<DataSet> GetInvAssmblyItemHdr(InvAssmblyItemHdr entity, string authParms)
        {
            var query = $" SELECT ITM_HDR.*,OWNER.OWNER_NAME_AR,OWNER.OWNER_NAME_EN,iim.IIM_NAME_AR,iim.IIM_NAME_EN" +
                $"  FROM INV_ASSMBLY_ITM_HDR ITM_HDR" +
                $"  LEFT OUTER JOIN GAS_OWNER OWNER ON ITM_HDR.IAIH_OWNER_SYS_ID = OWNER.OWNER_SYS_ID" +
                $"  LEFT OUTER  JOIN INV_INVENTORY_MASTER iim ON iim.IIM_SYS_ID = ITM_HDR.IAIH_INV_SYS_ID" +
                $"  WHERE ITM_HDR.IAIH_SYS_ID = :pcode OR :pcode = 0 ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pcode", entity.IAIH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetinvInvAssmblyItemDtls(invAssmblyItemHDtl entity, string authParms)
        {

            var query = $"select * from INV_ASSMBLY_ITM_DTL where  INV_ASSMBLY_ITM_DTL.IAID_HDR_SYS_ID= :pcode";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pcode", entity.IAID_HDR_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostInvAssmblyItemMasterDetails(InvAssmblyItmData entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            //entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
            entities.INVASSMBLYITMHDR.INS_USER = authP.UserCode.Value;
            entities.INVASSMBLYITMHDR.IAIH_V_CODE = authP.User_Act_PH;

            if (entities.INVASSMBLYITMHDR.IAIH_SYS_ID > 0)
                entities.INVASSMBLYITMHDR.STATE = (int)OperationType.Update;
            else entities.INVASSMBLYITMHDR.STATE = (int)OperationType.Add;
            // DTL
            for (int i = 0; i < entities.INVASSMBLYITMDTL.Count; i++)
            {
                entities.INVASSMBLYITMDTL[i].INS_USER = authP.UserCode;
                if (entities.INVASSMBLYITMDTL[i].IAID_SYS_ID > 0)
                    if (entities.INVASSMBLYITMDTL[i].STATE == 3)
                    {
                        entities.INVASSMBLYITMDTL[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.INVASSMBLYITMDTL[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.INVASSMBLYITMDTL[i].STATE = (int)OperationType.Add;
            }


            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("ASSMBLY_ITM_HDR_XML", new List<dynamic>() { entities.INVASSMBLYITMHDR });
            parameters.Add("ASSMBLY_ITM_DTL_XML", entities.INVASSMBLYITMDTL.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsyncWithOutPut("PRC_INV_ASSMBLY_ITM_H_d_XML", parameters, authParms);
        }
        public async Task<DataSet> DeleteInvAssmblyItemMasterDetails(InvAssmblyItemHdr entity, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_Item_Mast", new List<dynamic>() { entity });
            parameters.Add("xml_document_Btch", new List<dynamic>() { });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_ASSMBLY_ITM_H_d_XML", parameters, authParms);
        }
        public async Task<DataSet> GetLastCode(string type, string authParms)
        {
            var query = $"SELECT  NVL (MAX (TO_NUMBER (CASE WHEN REGEXP_LIKE (IAIH_NO, '^[0-9]+') THEN IAIH_NO ELSE '0' END)), 0) + 1 AS Code" +
                $" FROM INV_ASSMBLY_ITM_HDR  where IAIH_TYPE_ASM_DASM_A_D = '"+ type + "' and IAIH_V_CODE='"+ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH + "' ";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

    }
}


