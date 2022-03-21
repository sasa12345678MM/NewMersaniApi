using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class OwnerSetupRepository : IOwnerSetupRepo
    {


        public async Task<List<OwnerSetup>> GetOwnerSetup(int id, string authParms)
        {
            var query = $"SELECT * FROM GAS_OWNER  WHERE OWNER_SYS_ID = :pCode OR :pCode = 0";
            return await OracleDQ.GetDataAsync<OwnerSetup>(query, authParms, new { pCode = id });
        }
        public async Task<DataSet> PostOwnerSetup(List<OwnerSetup> entities, string authParms)
        {
            foreach (OwnerSetup entity in entities)
            {
                entity.INS_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.OWNER_SYS_ID > 0) entity.STATE = 2;
                else entity.STATE = 1;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_OWNER_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeletOwnerSetup(List<OwnerSetup> entities, string authParms)
        {
            foreach (OwnerSetup entity in entities)
            {
                entity.INS_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                entity.STATE = 3;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_OWNER_XML", entities.ToList<dynamic>(), authParms);
        }


        private OracleDynamicParameters GetDynamicParameters(OwnerSetup entity, string authParms, OperationType operationType)
        {
            var dyParam = new OracleDynamicParameters();
            if (operationType != OperationType.Add)
                dyParam.Add("P_OWNER_SYS_ID", OracleDbType.Int32, ParameterDirection.Input, entity.OWNER_SYS_ID);
            if (operationType != OperationType.Delete)
            {
                dyParam.Add("P_OWNER_CODE", OracleDbType.Varchar2, ParameterDirection.Input, entity.OWNER_CODE);
                dyParam.Add("P_OWNER_NAME_AR", OracleDbType.Varchar2, ParameterDirection.Input, entity.OWNER_NAME_AR);
                dyParam.Add("P_OWNER_NAME_EN", OracleDbType.Varchar2, ParameterDirection.Input, entity.OWNER_NAME_EN);
                dyParam.Add("P_OWNER_MOB", OracleDbType.Varchar2, ParameterDirection.Input, entity.OWNER_MOB);
                dyParam.Add("P_OWNER_EMAIL_ID", OracleDbType.Varchar2, ParameterDirection.Input, entity.OWNER_EMAIL_ID);
                dyParam.Add("P_OWNER_TEL", OracleDbType.Varchar2, ParameterDirection.Input, entity.OWNER_TEL);
                dyParam.Add("P_OWNER_FRZ_Y_N", OracleDbType.Varchar2, ParameterDirection.Input, entity.OWNER_FRZ_Y_N);
            }
            dyParam.Add("P_USER_ID", OracleDbType.Int32, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserCode);
            dyParam.Add("P_LANG", OracleDbType.Varchar2, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserLanguage);
            dyParam.Add("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
            dyParam.Add("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);

            return dyParam;
        }
 
        public async Task<DataSet> getOwnerByMobile(string mobile, string authParms)
        {
            var query = $"SELECT GAS_OWNER.*" +
                $"                 FROM GAS_OWNER" +
                $"                WHERE(GAS_OWNER.OWNER_MOB = '{mobile}')";

            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetGasCInsCompany(gasOwnerInsCo entities, string authParms)
        {
            var query = $"SELECT GAS_OWNER_INS_CO.*," +
               $"       POS_INSURANCE_CMP.PIC_NAME_AR," +
               $"       POS_INSURANCE_CMP.PIC_NAME_EN" +
               $"  FROM GAS_OWNER_INS_CO" +
               $"       INNER JOIN POS_INSURANCE_CMP" +
               $"          ON POS_INSURANCE_CMP.PIC_SYS_ID =" +
               $"                GAS_OWNER_INS_CO.GOIC_INS_CO_SYS_ID" +
               $" WHERE GOIC_OWNER_SYS_ID = :PGOIC_OWNER_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PGOIC_OWNER_SYS_ID", entities.GOIC_OWNER_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostGasCInsCompany(List<gasOwnerInsCo> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);

            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].CURR_USER = authP.UserCode;
                if (entities[i].GOIC_SYS_ID > 0)
                    if (entities[i].STATE == 3)
                    {
                        entities[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities[i].STATE = (int)OperationType.Add;
            }


            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_d", entities.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_GAS_OWNER_INS_CO_XML", parameters, authParms);
        }

    }
}