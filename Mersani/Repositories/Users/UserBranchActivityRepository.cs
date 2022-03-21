using System.Data;
using Mersani.Oracle;
using Mersani.models.Users;
using System.Threading.Tasks;
using Mersani.Interfaces.Users;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

namespace Mersani.Repositories.Users
{
    public class UserBranchActivityRepository : IUserBranchActivityRepo
    {
        public async Task<List<UserBranchActivity>> GetUserBranchActivity(int id, string authParms)
        {
            var query = $"SELECT UBA.*, " +
                $"(GAM.FAC_NAME_AR || '-' || GCB.CB_NAME_AR) AS ACTIVITY_NAME_AR, " +
                $"(GAM.FAC_NAME_EN || '-' || GCB.CB_NAME_EN) AS ACTIVITY_NAME_EN FROM GAS_USR_BR_ACTV UBA " +
                $"JOIN GAS_BR_ACTV FACB ON FACB.FAC_SYS_ID = UBA.UBA_ACV_SYS_ID " +
                $"JOIN GAS_ACTIVITY_MASTER GAM ON GAM.FAC_CODE = FACB.FAC_ACTIVITY_CODE " +
                $"JOIN GAS_COMPANY_BRANCHES GCB ON GCB.CB_SYS_ID = FACB.FAC_BR_SYS_ID " +
                $"WHERE UBA.UBA_USR_CODE = :pUBA_USR_CODE OR :pUBA_USR_CODE = 0";
            return await OracleDQ.GetDataAsync<UserBranchActivity>(query, authParms, new { pUBA_USR_CODE = id });
        }

        public async Task<bool> PostNewUserBranchActivity(UserBranchActivity userCompanyBranch, string authParms)
        {
            string storedProc;
            OperationType operationType;
            if (userCompanyBranch.UBA_SYS_ID > 0)
            {
                storedProc = "PRC_GAS_USR_BR_ACTV_UPD";
                operationType = OperationType.Update;
            }
            else
            {
                storedProc = "PRC_GAS_USR_BR_ACTV_INS";
                operationType = OperationType.Add;
            }
            var dyParam = GetDynamicParameters(userCompanyBranch, authParms, operationType);
            return await OracleDQ.PostDataAsync(storedProc, authParms, dyParam, commandType: CommandType.StoredProcedure);
        }
         
        public async Task<bool> DeleteUserBranchActivity(int id, string authParms)
        {
            var dyParam = GetDynamicParameters(new UserBranchActivity() { UBA_SYS_ID = id }, authParms, OperationType.Delete);
            return await OracleDQ.PostDataAsync("PRC_GAS_USR_BR_ACTV_DEL", authParms, dyParam, commandType: CommandType.StoredProcedure);
        }
        private OracleDynamicParameters GetDynamicParameters(UserBranchActivity entity, string authParms, OperationType operationType)
        {
            var dyParam = new OracleDynamicParameters();
            if (operationType != OperationType.Add)
                dyParam.Add("P_UBA_SYS_ID", OracleDbType.Int32, ParameterDirection.Input, entity.UBA_SYS_ID);

            if (operationType != OperationType.Delete)
            {
                dyParam.Add("P_UBA_ACV_SYS_ID", OracleDbType.Int32, ParameterDirection.Input, entity.UBA_ACV_SYS_ID);
                dyParam.Add("P_UBA_USR_CODE", OracleDbType.Int32, ParameterDirection.Input, entity.UBA_USR_CODE);
                dyParam.Add("P_UBA_FRZ_Y_N", OracleDbType.Char, ParameterDirection.Input, entity.UBA_FRZ_Y_N);
                dyParam.Add("P_UBA_FRZ_REASON", OracleDbType.Varchar2, ParameterDirection.Input, entity.UBA_FRZ_REASON);
            }
            dyParam.Add("P_USER_ID", OracleDbType.Int32, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserCode);
            dyParam.Add("P_LANG", OracleDbType.Varchar2, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserLanguage);
            dyParam.Add("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
            dyParam.Add("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);

            return dyParam;
        }

        public Task GetpharmcyActivity(int id, string authParms)
        {
            throw new System.NotImplementedException();
        }
        //public async Task<DataSet> BulkUserBranchActivityXML(List<UserBranchActivity> entities, string authParms)
        //{
        //    foreach (var entity in entities)
        //    {
        //        if (entity.UBA_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
        //        else entity.STATE = (int)OperationType.Add;
        //        entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
        //    }

        //    return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_USR_BR_ACTV_XML", entities.ToList<dynamic>(), authParms);
        //}

        //public async Task<DataSet> DeleteUserBranchActivityxml(int id, string authParms)
        //{
        //    return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_USR_BR_ACTV_XML", new List<dynamic>() {
        //        new UserPharmacies() { UBA_SYS_ID = id, STATE = (int)OperationType.Delete }
        //    }, authParms);
        //}
    }
}
