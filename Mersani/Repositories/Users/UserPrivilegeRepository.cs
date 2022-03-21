using Mersani.Interfaces.Users;
using Mersani.models.Users;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Users
{
    public class UserPrivilegeRepository : IUserPrivilegeRepo
    {
        //public async Task<List<UserPrivilege>> GetUserPrivilegeByActivity(int userActivityId, string authParms)
        //{
        //    var query = $"SELECT PRV.UCBF_SYS_ID, PRV.UCBF_INSERT_ALLOWED_Y_N, PRV.UCBF_UPDATE_ALLOWED_Y_N, PRV.UCBF_DELETE_ALLOWED_Y_N, " +
        //        $"PRV.UCBF_QUERY_ALLOWED_Y_N, PRV.UCBF_RUN_REP_ALLOWED_Y_N, PRV.UBA_SYS_ID, PRV.MNU_CODE, MNU.MNU_LABEL_AR, MNU.MNU_LABEL_EN, " +
        //        $"com.MNU_LABEL_AR as parent_ar, com.MNU_LABEL_en as parent_en, com.MNU_CODE as parent_code " +
        //        $"FROM GAS_USR_PRIVILEGE PRV JOIN GAS_MNU MNU ON MNU.MNU_CODE = PRV.MNU_CODE  AND MNU.MNU_TYPE = 'C' " +
        //        $"LEFT JOIN GAS_MNU com ON com.MNU_CODE = mnu.MNU_PARENT AND com.MNU_TYPE = 'P' " +
        //        $"WHERE PRV.UBA_SYS_ID = :pUBA_SYS_ID";
        //    return await OracleDQ.GetDataAsync<UserPrivilege>(query, authParms, new { pUBA_SYS_ID = userActivityId });
        //}

        //public async Task<List<UserPrivilege>> GetUserPrivilege(int id, string authParms)
        //{
        //    var query = $"SELECT PRV.UCBF_SYS_ID, PRV.UCBF_INSERT_ALLOWED_Y_N, PRV.UCBF_UPDATE_ALLOWED_Y_N, PRV.UCBF_DELETE_ALLOWED_Y_N, " +
        //        $"PRV.UCBF_QUERY_ALLOWED_Y_N, PRV.UCBF_RUN_REP_ALLOWED_Y_N, PRV.UBA_SYS_ID, PRV.MNU_CODE, MNU.MNU_LABEL_AR, MNU.MNU_LABEL_EN " +
        //        $"FROM GAS_USR_PRIVILEGE PRV JOIN GAS_MNU MNU ON MNU.MNU_CODE = PRV.MNU_CODE " +
        //        $"WHERE PRV.UCBF_SYS_ID = :pUCBF_SYS_ID";
        //    return await OracleDQ.GetDataAsync<UserPrivilege>(query, authParms, new { pUCBF_SYS_ID = id });
        //}

        //public async Task<bool> PostNewUserPrivilege(UserPrivilege userComBrPrivilege, string authParms)
        //{
        //    string storedProc;
        //    OperationType operationType;
        //    if (userComBrPrivilege.UCBF_SYS_ID > 0)
        //    {
        //        storedProc = "PRC_GAS_USR_PRIVILEGE_UPD";
        //        operationType = OperationType.Update;
        //    }
        //    else
        //    {
        //        storedProc = "PRC_GAS_USR_PRIVILEGE_INS";
        //        operationType = OperationType.Add;
        //    }
        //    var dyParam = GetDynamicParameters(userComBrPrivilege, authParms, operationType);
        //    return await OracleDQ.PostDataAsync(storedProc, authParms, dyParam, commandType: CommandType.StoredProcedure);
        //}

        //public async Task<bool> DeleteUserPrivilege(int id, string authParms)
        //{
        //    var dyParam = GetDynamicParameters(new UserPrivilege() { UCBF_SYS_ID = id }, authParms, OperationType.Delete);
        //    return await OracleDQ.PostDataAsync("PRC_GAS_USR_PRIVILEGE_DEL", authParms, dyParam, commandType: CommandType.StoredProcedure);
        //}

        //private OracleDynamicParameters GetDynamicParameters(UserPrivilege entity, string authParms, OperationType operationType)
        //{
        //    var dyParam = new OracleDynamicParameters();
        //    if (operationType == OperationType.Add)
        //        dyParam.Add("P_UCBF_UCB_SYS_ID", OracleDbType.Int32, ParameterDirection.Input, entity.UCBF_SYS_ID);
        //    if (operationType == OperationType.Update || operationType == OperationType.Delete)
        //        dyParam.Add("P_UCBF_SYS_ID", OracleDbType.Int32, ParameterDirection.Input, entity.UCBF_SYS_ID);

        //    if (operationType != OperationType.Delete)
        //    {
        //        dyParam.Add("P_UBA_SYS_ID", OracleDbType.Int32, ParameterDirection.Input, entity.UBA_SYS_ID);
        //        dyParam.Add("P_MNU_CODE", OracleDbType.Int32, ParameterDirection.Input, entity.MNU_CODE);

        //        dyParam.Add("P_UCBF_INSERT_ALLOWED_Y_N", OracleDbType.Varchar2, ParameterDirection.Input, entity.UCBF_INSERT_ALLOWED_Y_N);
        //        dyParam.Add("P_UCBF_DELETE_ALLOWED_Y_N", OracleDbType.Varchar2, ParameterDirection.Input, entity.UCBF_DELETE_ALLOWED_Y_N);
        //        dyParam.Add("P_UCBF_UPDATE_ALLOWED_Y_N", OracleDbType.Varchar2, ParameterDirection.Input, entity.UCBF_UPDATE_ALLOWED_Y_N);
        //        dyParam.Add("P_UCBF_QUERY_ALLOWED_Y_N", OracleDbType.Varchar2, ParameterDirection.Input, entity.UCBF_QUERY_ALLOWED_Y_N);
        //        dyParam.Add("P_UCBF_RUN_REP_ALLOWED_Y_N", OracleDbType.Varchar2, ParameterDirection.Input, entity.UCBF_RUN_REP_ALLOWED_Y_N);
        //    }
        //    dyParam.Add("P_USER_ID", OracleDbType.Int32, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserCode);
        //    dyParam.Add("P_LANG", OracleDbType.Varchar2, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserLanguage);
        //    dyParam.Add("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
        //    dyParam.Add("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);

        //    return dyParam;
        //}

        public async Task<List<UserPrivilege>> CheckUserPrivilegeByUrl(UserPrivilege userPrivilege, string authParms)
        {
            var query = $"SELECT PRV.UCBF_SYS_ID, PRV.UCBF_INSERT_ALLOWED_Y_N, PRV.UCBF_UPDATE_ALLOWED_Y_N, PRV.UCBF_DELETE_ALLOWED_Y_N, " +
                $"PRV.UCBF_QUERY_ALLOWED_Y_N, PRV.UCBF_RUN_REP_ALLOWED_Y_N, PRV.UBA_SYS_ID, PRV.MNU_CODE, MNU.MNU_LABEL_AR, MNU.MNU_LABEL_EN, MNU.MNU_NAME " +
                $"FROM GAS_USR_PRIVILEGE PRV JOIN GAS_MNU MNU ON MNU.MNU_CODE = PRV.MNU_CODE " +
                $"WHERE PRV.UBA_SYS_ID = :pUBA_SYS_ID AND MNU.MNU_PATH LIKE '%{userPrivilege.MNU_PATH}%'";
            return await OracleDQ.GetDataAsync<UserPrivilege>(query, authParms, new { pUBA_SYS_ID = userPrivilege.UBA_SYS_ID });
        }

        public async Task<DataSet> BulkUserPrivileges(List<UserPrivilege> privileges, string authParms)
        {
            foreach (var entity in privileges)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.UCBF_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_USR_PRIVILEGE_XML", privileges.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteUserPrivilege(UserPrivilege privilege, string authParms)
        {
            privilege.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_USR_PRIVILEGE_XML", new List<dynamic>() { privilege }, authParms);
        }

        public async Task<DataSet> GetUserPrivilegesByActivity(UserPrivilege privilege, string authParms)
        {
            var query = $"SELECT PRV.UCBF_SYS_ID, PRV.UCBF_INSERT_ALLOWED_Y_N, PRV.UCBF_UPDATE_ALLOWED_Y_N, PRV.UCBF_DELETE_ALLOWED_Y_N, " +
                $"PRV.UCBF_QUERY_ALLOWED_Y_N, PRV.UCBF_RUN_REP_ALLOWED_Y_N, PRV.UBA_SYS_ID, PRV.MNU_CODE, MNU.MNU_LABEL_AR, MNU.MNU_LABEL_EN, " +
                $"com.MNU_LABEL_AR as parent_ar, com.MNU_LABEL_en as parent_en, com.MNU_CODE as parent_code " +
                $"FROM GAS_USR_PRIVILEGE PRV JOIN GAS_MNU MNU ON MNU.MNU_CODE = PRV.MNU_CODE  AND MNU.MNU_TYPE = 'C' " +
                $"LEFT JOIN GAS_MNU com ON com.MNU_CODE = mnu.MNU_PARENT AND com.MNU_TYPE = 'P' " +
                $"WHERE PRV.UBA_SYS_ID = :pUBA_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pUBA_SYS_ID", privilege.UBA_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetUserPrivilegeById(UserPrivilege privilege, string authParms)
        {
            var query = $"SELECT PRV.UCBF_SYS_ID, PRV.UCBF_INSERT_ALLOWED_Y_N, PRV.UCBF_UPDATE_ALLOWED_Y_N, PRV.UCBF_DELETE_ALLOWED_Y_N, " +
                $"PRV.UCBF_QUERY_ALLOWED_Y_N, PRV.UCBF_RUN_REP_ALLOWED_Y_N, PRV.UBA_SYS_ID, PRV.MNU_CODE, MNU.MNU_LABEL_AR, MNU.MNU_LABEL_EN " +
                $"FROM GAS_USR_PRIVILEGE PRV JOIN GAS_MNU MNU ON MNU.MNU_CODE = PRV.MNU_CODE " +
                $"WHERE PRV.UCBF_SYS_ID = :pUCBF_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pUCBF_SYS_ID", privilege.UCBF_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
    }
}
