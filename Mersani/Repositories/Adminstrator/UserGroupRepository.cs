using System.Data;
using Mersani.Oracle;
using System.Collections.Generic;
using Mersani.models.Administrator;
using Oracle.ManagedDataAccess.Client;
using Mersani.Interfaces.Administrator;

namespace Mersani.Repositories.Adminstrator
{
    public class UserGroupRepository : IUserGroupRepo
    {
        public List<UserGroup> GetUserGroup(int id, string authParms)
        {
            var query = $"SELECT * FROM GAS_USRGRP WHERE USRGRP_CODE = :pUSRGRP_CODE OR :pUSRGRP_CODE = 0";
            return OracleDQ.GetData<UserGroup>(query, authParms, new { pUSRGRP_CODE = id });
        }

        public bool PostNewUserGroup(UserGroup userGroup, string authParms)
        {
            string storedProc = "";
            OperationType operationType = OperationType.Other;
            if (userGroup.USRGRP_CODE > 0)
            {
                storedProc = "PRC_GAS_USRGRP_UPD";
                operationType = OperationType.Update;
            }
            else
            {
                storedProc = "PRC_GAS_USRGRP_INS";
                operationType = OperationType.Add;
            }
            var dyParam = GetDynamicParameters(userGroup, authParms, operationType);
            return OracleDQ.PostData(storedProc, authParms, dyParam, commandType: CommandType.StoredProcedure);
        }
        public bool DeleteUserGroup(int id, string authParms)
        {
            var dyParam = GetDynamicParameters(new UserGroup() { USRGRP_CODE = id }, authParms, OperationType.Delete);
            return OracleDQ.PostData("PRC_GAS_USRGRP_DEL", authParms, dyParam, commandType: CommandType.StoredProcedure);
        }

        private OracleDynamicParameters GetDynamicParameters(UserGroup userGroup, string authParms, OperationType operationType)
        {
            var dyParam = new OracleDynamicParameters();
            if (operationType != OperationType.Add)
                dyParam.Add("P_USRGRP_CODE", OracleDbType.Int32, ParameterDirection.Input, userGroup.USRGRP_CODE);

            if (operationType != OperationType.Delete)
            {
                dyParam.Add("P_USRGRP_NAME_AR", OracleDbType.Varchar2, ParameterDirection.Input, userGroup.USRGRP_NAME_AR);
                dyParam.Add("P_USRGRP_NAME_EN", OracleDbType.Varchar2, ParameterDirection.Input, userGroup.USRGRP_NAME_EN);
            }
            dyParam.Add("P_USER_ID", OracleDbType.Int32, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserCode);
            dyParam.Add("P_LANG", OracleDbType.Varchar2, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserLanguage);
            dyParam.Add("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
            dyParam.Add("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);

            return dyParam;
        }
    }
}
