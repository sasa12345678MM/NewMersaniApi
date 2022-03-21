using Mersani.models.Administrator;
using Mersani.Interfaces.Administrator;
using System.Collections.Generic;
using Mersani.Oracle;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Mersani.Repositories.Adminstrator
{
    public class UserGroupMenuRepository : IUserGroupMenuRepo
    {
        public List<UserGroupMenu> GetMenuesByUserGroup(int userGroupId, string authParms)
        {
            var query = $"SELECT UGM.USGRMN_SYS_ID, UGM.USRGRP_CODE, UGM.MNU_CODE, UG.USRGRP_NAME_AR, UG.USRGRP_NAME_EN, MN.MNU_LABEL_AR, MN.MNU_LABEL_EN, " +
                $" MN.MNU_PARENT, MNP.MNU_LABEL_AR AS MNU_PARENT_AR, MNP.MNU_LABEL_EN AS MNU_PARENT_EN,MN.MNU_ORD " +
                $" FROM GAS_USRGRP_MNU UGM" +
                $" JOIN GAS_USRGRP UG ON UG.USRGRP_CODE = UGM.USRGRP_CODE" +
                $" JOIN GAS_MNU MN ON MN.MNU_CODE = UGM.MNU_CODE " +
                $" JOIN GAS_MNU MNP ON MNP.MNU_CODE = MN.MNU_PARENT " +
                $" WHERE UGM.USRGRP_CODE = :pUSRGRP_CODE";
            return OracleDQ.GetData<UserGroupMenu>(query, authParms, new { pUSRGRP_CODE = userGroupId });
        }

        public List<UserGroupMenu> GetUserGroupMenu(int id, string authParms)
        {
            var query = $"SELECT UGM.USGRMN_SYS_ID, UGM.USRGRP_CODE, UGM.MNU_CODE, UG.USRGRP_NAME_AR, UG.USRGRP_NAME_EN, MN.MNU_LABEL_AR, MN.MNU_LABEL_EN" +
                $" FROM GAS_USRGRP_MNU UGM" +
                $" JOIN GAS_USRGRP UG ON UG.USRGRP_CODE = UGM.USRGRP_CODE" +
                $" JOIN GAS_MNU MN ON MN.MNU_CODE = UGM.MNU_CODE" +
                $" WHERE UGM.USGRMN_SYS_ID = :pUSGRMN_SYS_ID OR :pUSGRMN_SYS_ID = 0";
            return OracleDQ.GetData<UserGroupMenu>(query, authParms, new { pUSGRMN_SYS_ID = id });
        }

        public bool PostNewUserGroupMenu(UserGroupMenu userGroupMenu, string authParms)
        {
            string storedProc = "";
            OperationType operationType = OperationType.Other;
            if (userGroupMenu.USGRMN_SYS_ID > 0)
            {
                storedProc = "PRC_GAS_USRGRP_MNU_UPD";
                operationType = OperationType.Update;
            }
            else
            {
                storedProc = "PRC_GAS_USRGRP_MNU_INS";
                operationType = OperationType.Add;
            }
            var dyParam = GetDynamicParameters(userGroupMenu, authParms, operationType);
            return OracleDQ.PostData(storedProc, authParms, dyParam, commandType: CommandType.StoredProcedure);
        }

        public bool DeleteUserGroupMenu(int id, string authParms)
        {
            var dyParam = GetDynamicParameters(new UserGroupMenu() { USGRMN_SYS_ID = id }, authParms, OperationType.Delete);
            return OracleDQ.PostData("PRC_GAS_USRGRP_MNU_DEL", authParms, dyParam, commandType: CommandType.StoredProcedure);
        }

        private OracleDynamicParameters GetDynamicParameters(UserGroupMenu userGroupMenu, string authParms, OperationType operationType)
        {
            var dyParam = new OracleDynamicParameters();
            if (operationType != OperationType.Add)
                dyParam.Add("P_USGRMN_SYS_ID", OracleDbType.Int32, ParameterDirection.Input, userGroupMenu.USGRMN_SYS_ID);

            if (operationType != OperationType.Delete)
            {
                dyParam.Add("P_USRGRP_CODE", OracleDbType.Int32, ParameterDirection.Input, userGroupMenu.USRGRP_CODE);
                dyParam.Add("P_MNU_CODE", OracleDbType.Int32, ParameterDirection.Input, userGroupMenu.MNU_CODE);
            }
            dyParam.Add("P_USER_ID", OracleDbType.Int32, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserCode);
            dyParam.Add("P_LANG", OracleDbType.Varchar2, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserLanguage);
            dyParam.Add("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
            dyParam.Add("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);

            return dyParam;
        }
    }
}
