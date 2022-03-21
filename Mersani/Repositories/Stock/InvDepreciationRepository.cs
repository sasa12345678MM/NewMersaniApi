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
    public class InvDepreciationRepository : InvDepreciationRepo
    {
        public async Task<DataSet> GetAdjstDepMstr(invAdjstDepMstr entity, string authParms)
        {
            var query = $"select mstr.*,usr.USR_FULL_NAME_AR,usr.USR_FULL_NAME_EN,inventory.IIM_NAME_AR ,inventory.IIM_NAME_En from INV_ADJST_DEP_MSTR  mstr" +
                $" inner join INV_INVENTORY_MASTER inventory on mstr.IADM_INV_SYS_ID = inventory.IIM_SYS_ID " +
                $"inner join GAS_USR usr on usr.USR_CODE = mstr.IADM_CMTE_MNGR_USR_CODE  " +
                $"WHERE mstr.IADM_SYS_ID = :pIADM_SYS_ID OR :pIADM_SYS_ID = 0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pIADM_SYS_ID", entity.IADM_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetinvAdjstDepDtls(invAdjstDepDtls entity, string authParms)
        {
            var query = $"select * from INV_ADJST_DEP_DTLS WHERE INV_ADJST_DEP_DTLS.IADD_IADM_SYS_ID = :pIADM_SYS_ID OR :pIADM_SYS_ID = 0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pIADD_IADM_SYS_ID", entity.IADD_IADM_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> DeleteAdjstDepMasterDetails(invAdjstDepMstr entity, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entity.STATE = 3;
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_Mstr", new List<dynamic>() { entity });
            parameters.Add("xml_document_Dtl", new List<dynamic>() { });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_ADJST_DEP_MSTR_DTL_XML", parameters, authParms);
        }

     

        public async Task<DataSet> PostAdjstDepMasterDetails(AdjstDepData entities, string authParms)
        {
            //MSTR
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            //entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
            entities.INVADJSTDEPMSTR.INS_USER = authP.UserCode.Value;
            entities.INVADJSTDEPMSTR.IADM_V_CODE = authP.User_Act_PH;

            if (entities.INVADJSTDEPMSTR.IADM_SYS_ID > 0)
                entities.INVADJSTDEPMSTR.STATE = (int)OperationType.Update;
            else entities.INVADJSTDEPMSTR.STATE = (int)OperationType.Add;
            // DTL
            for (int i = 0; i < entities.INVADJSTDEPDTLS.Count; i++)
            {
                entities.INVADJSTDEPDTLS[i].INS_USER = authP.UserCode;
                if (entities.INVADJSTDEPDTLS[i].IADD_SYS_ID > 0)
                    if (entities.INVADJSTDEPDTLS[i].STATE == 3)
                    {
                        entities.INVADJSTDEPDTLS[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.INVADJSTDEPDTLS[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.INVADJSTDEPDTLS[i].STATE = (int)OperationType.Add;
            }


            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_Mstr", new List<dynamic>() { entities.INVADJSTDEPMSTR });
            parameters.Add("xml_document_Dtl", entities.INVADJSTDEPDTLS.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_ADJST_DEP_MSTR_DTL_XML", parameters, authParms);
        }

        public async Task<DataSet> approvalinvAdjstDepMstr(List<invAdjstDepMstr> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            foreach (invAdjstDepMstr entity in entities)
            {
                entity.INS_USER = authP.UserCode.Value;
                entity.IADM_V_CODE = authP.User_Act_PH;

            }
            parameters.Add("xml_document_Mstr", entities.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_POST_ADJST_DEP_XML", parameters, authParms);
        }
    }
}
