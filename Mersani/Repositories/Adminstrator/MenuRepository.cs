using System.Data;
using Mersani.Oracle;
using System.Collections.Generic;
using Mersani.models.Administrator;
using Oracle.ManagedDataAccess.Client;
using Mersani.Interfaces.Administrator;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class MenuRepository : IMenuRepo
    {
        public List<Menu> GetMenu(int id, string authParms)
        {
            var query = $"SELECT * FROM GAS_MNU WHERE MNU_CODE = :pMNU_CODE OR :pMNU_CODE = 0 ";
            return OracleDQ.GetData<Menu>(query, authParms, new { pMNU_CODE = id });
        }
        public bool PostNewMenu(Menu menu, string authParms)
        {
            var dyParam = GetDynamicParameters(menu, authParms, OperationType.Add);
            return OracleDQ.PostData("PRC_GAS_MNU_INS", authParms, dyParam, commandType: CommandType.StoredProcedure);
        }
        public bool UpdateMenu(int id, Menu menu, string authParms)
        {
            var dyParam = GetDynamicParameters(menu, authParms, OperationType.Update);
            return OracleDQ.PostData("PRC_GAS_MNU_UPD", authParms, dyParam, commandType: CommandType.StoredProcedure);
        }
        public bool DeleteMenu(int id, string authParms)
        {
            var dyParam = GetDynamicParameters(new Menu() { MNU_CODE = id }, authParms, OperationType.Delete);
            return OracleDQ.PostData("PRC_GAS_MNU_DEL", authParms, dyParam, commandType: CommandType.StoredProcedure);
        }

        public List<Menu> GetUserMenu(int userId, string authParms)
        {
            var dyParam = GetDynamicParameters(new Menu() { MNU_CODE = userId }, authParms, OperationType.Other);
            return OracleDQ.GetData<Menu>("PRC_GET_MNU_USR", authParms, dyParam, commandType: CommandType.StoredProcedure);
        }

        private OracleDynamicParameters GetDynamicParameters(Menu menu, string authParms, OperationType operationType)
        {
            var dyParam = new OracleDynamicParameters();
            if (operationType != OperationType.Add && operationType != OperationType.Other)
            {
                dyParam.Add("P_MNU_CODE", OracleDbType.Int32, ParameterDirection.Input, menu.MNU_CODE);
            }
            if (operationType != OperationType.Delete && operationType != OperationType.Other)
            {
                dyParam.Add("P_MNU_PARENT", OracleDbType.Int32, ParameterDirection.Input, menu.MNU_PARENT);
                dyParam.Add("P_MNU_NAME", OracleDbType.NVarchar2, ParameterDirection.Input, operationType == OperationType.Add ? "NO_MENU" : menu.MNU_NAME);
                dyParam.Add("P_MNU_PATH", OracleDbType.NVarchar2, ParameterDirection.Input, menu.MNU_PATH);
                dyParam.Add("P_MNU_LABEL_AR", OracleDbType.Varchar2, ParameterDirection.Input, menu.MNU_LABEL_AR);
                dyParam.Add("P_MNU_LABEL_EN", OracleDbType.Varchar2, ParameterDirection.Input, menu.MNU_LABEL_EN);
                dyParam.Add("P_MNU_ORD", OracleDbType.Int32, ParameterDirection.Input, menu.MNU_ORD);
                dyParam.Add("P_MNU_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, menu.MNU_TYPE);
                dyParam.Add("P_MNU_PAGE_REPORT_P_R", OracleDbType.Varchar2, ParameterDirection.Input, menu.MNU_PAGE_REPORT_P_R);
            }

            if (operationType == OperationType.Other)
            {
                dyParam.Add("P_MNU_ID", OracleDbType.RefCursor, ParameterDirection.Output);
                dyParam.Add("P_USER_ID", OracleDbType.Int32, ParameterDirection.Input, menu.MNU_CODE > 0 ? menu.MNU_CODE : OracleDQ.GetAuthenticatedUserObject(authParms).UserCode);
            }
            else
            {
                dyParam.Add("P_USER_ID", OracleDbType.Int32, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserCode);
            }

            dyParam.Add("P_LANG", OracleDbType.Varchar2, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserLanguage.ToUpper());
            dyParam.Add("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.InputOutput);
            dyParam.Add("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.InputOutput);

            return dyParam;
        }

        public async Task<DataSet> GetReportMenus(int menuCode, string authParms)
        {
            var query = $"SELECT reps.* FROM GAS_MNU_REPORTS reps " +
                $" JOIN GAS_MNU_REPORT_USERS usrs ON reps.MNURPT_SYS_ID = usrs.GMRU_MNURPT_SYS_ID " +
                $" WHERE reps.MNURPT_MNU_CODE = :pCode AND usrs.GMRU_USR_CODE = :pUserCode";
            var parms = new List<OracleParameter>() { new OracleParameter("pCode", menuCode), new OracleParameter("pUserCode", OracleDQ.GetAuthenticatedUserObject(authParms).UserCode) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetReportMenuDetails(int menuCode, string authParms)
        {
            var query = $"SELECT RDTL.RDTL_MNU_CODE, RDTL.RDTL_SHOW_Y_N, GNRL.GRP_SYS_ID, GNRL.GRP_NAME_AR, GNRL.GRP_NAME_EN, " +
                $" GNRL.GRP_CONTROLNAME, GNRL.GRP_TYPE_N_T_D_L, GNRL.GRP_APPCODE, GNRL.GRP_NOTES " +
                $" FROM GAS_MNU_REPORT_PARM RDTL " +
                $" JOIN GAS_GNRL_REPORTS_PARM GNRL ON RDTL.RDTL_GRP_SYS_ID = GNRL.GRP_SYS_ID " +
                $" WHERE RDTL.RDTL_MNU_CODE = :pCode AND RDTL.RDTL_SHOW_Y_N = 'Y' " +
                $" ORDER BY RDTL.RDTL_ORDER ";
            var parms = new List<OracleParameter>() { new OracleParameter("pCode", menuCode) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
    }
}
