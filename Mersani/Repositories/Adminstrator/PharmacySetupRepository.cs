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
    public class PharmacySetupRepository : IPharmacySetupRepo
    {
        public async Task<List<PharmacySetup>> GetPharmacySetup(int id, string authParms)
        {
            var query = $"SELECT * FROM GAS_PHARMACY  WHERE PHARM_SYS_ID = :pCode OR :pCode = 0";

            return await OracleDQ.GetDataAsync<PharmacySetup>(query, authParms, new { pCode = id });
        }
        public async Task<List<PharmacySetup>> GetOwnerPharmacySetup(int id, string authParms)
        {
            var query = $"SELECT * FROM GAS_PHARMACY  WHERE OWNER_SYS_ID = :pCode OR :pCode = 0";
            return await OracleDQ.GetDataAsync<PharmacySetup>(query, authParms, new { pCode = id });
        }
        public async Task<DataSet> PostPharmacySetup(List<PharmacySetup> entities, string authParms)
        {
            foreach (PharmacySetup entity in entities)
            {
                entity.INS_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.PHARM_SYS_ID > 0) entity.STATE = 2;
                else entity.STATE = 1;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_PHARMACY_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeletPharmacySetup(List<PharmacySetup> entities, string authParms)
        {
            foreach (PharmacySetup entity in entities)
            {
                entity.INS_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                entity.STATE = 3;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_PHARMACY_XML", entities.ToList<dynamic>(), authParms);
        }
     


        private OracleDynamicParameters GetDynamicParameters(PharmacySetup entity, string authParms, OperationType operationType)
        {
            var dyParam = new OracleDynamicParameters();
            if (operationType != OperationType.Add)
                dyParam.Add("P_PHARM_SYS_ID", OracleDbType.Int32, ParameterDirection.Input, entity.PHARM_SYS_ID);
            if (operationType != OperationType.Delete)
            {
                dyParam.Add("P_PHARM_CODE", OracleDbType.Varchar2, ParameterDirection.Input, entity.PHARM_CODE);
                dyParam.Add("P_OWNER_SYS_ID", OracleDbType.Int32, ParameterDirection.Input, entity.OWNER_SYS_ID);
                dyParam.Add("P_PHARM_NAME_AR", OracleDbType.Varchar2, ParameterDirection.Input, entity.PHARM_NAME_AR);
                dyParam.Add("P_PHARM_NAME_EN", OracleDbType.Varchar2, ParameterDirection.Input, entity.PHARM_NAME_EN);
                dyParam.Add("P_PHARM_LOCATION", OracleDbType.Varchar2, ParameterDirection.Input, entity.PHARM_LOCATION);
                dyParam.Add("P_PHARM_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, entity.PHARM_PHONE);
                dyParam.Add("P_PHARM_EMAIL", OracleDbType.Varchar2, ParameterDirection.Input, entity.PHARM_EMAIL);
                dyParam.Add("P_PHARM_WEBSITE", OracleDbType.Varchar2, ParameterDirection.Input, entity.PHARM_WEBSITE);
                dyParam.Add("P_PHARM_FRZ_Y_N", OracleDbType.Varchar2, ParameterDirection.Input, entity.PHARM_FRZ_Y_N);
                dyParam.Add("P_GAS_CITY_ID", OracleDbType.Int32, ParameterDirection.Input, entity.GAS_CITY_ID);
                dyParam.Add("P_GOOGLE_MAP_DIR", OracleDbType.Varchar2, ParameterDirection.Input, entity.GOOGLE_MAP_DIR);
                dyParam.Add("PHARM_EMAIL_Y_N", OracleDbType.Varchar2, ParameterDirection.Input, entity.PHARM_EMAIL_Y_N);
                dyParam.Add("PHARM_SMS_Y_N", OracleDbType.Varchar2, ParameterDirection.Input, entity.PHARM_SMS_Y_N);
                dyParam.Add("PHARM_MOB", OracleDbType.Varchar2, ParameterDirection.Input, entity.PHARM_MOB);
                dyParam.Add("PHARM_LAT", OracleDbType.Int32, ParameterDirection.Input, entity.PHARM_LAT);
                dyParam.Add("PHARM_LONG", OracleDbType.Int32, ParameterDirection.Input, entity.PHARM_LONG);
                dyParam.Add("PHARM_SHOW_WEB_MOB_Y_N", OracleDbType.Varchar2, ParameterDirection.Input, entity.PHARM_SHOW_WEB_MOB_Y_N);
            }
            dyParam.Add("P_USER_ID", OracleDbType.Int32, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserCode);
            dyParam.Add("P_LANG", OracleDbType.Varchar2, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserLanguage);
            dyParam.Add("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
            dyParam.Add("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);

            return dyParam;
        }

        public async Task<DataSet> GetLastCode(int ownersysID,string authParms)
        {
            var query = $"SELECT  NVL (MAX (TO_NUMBER (CASE WHEN REGEXP_LIKE (PHARM_CODE, '^[0-9]+') THEN PHARM_CODE ELSE '0' END)), 0) + 1 AS Code" +
                $" FROM GAS_PHARMACY  where GAS_PHARMACY.OWNER_SYS_ID ="+ ownersysID + "";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

    }
}
