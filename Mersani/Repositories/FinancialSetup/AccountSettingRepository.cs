using Mersani.Interfaces.FinancialSetup;
using Mersani.models.FinancialSetup;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.FinancialSetup
{
    public class AccountSettingRepository : IAccountSettingRepo
    {
        public async Task<DataSet> BulkInsertUpdateAccountSetting(List<AccountSetting> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.ACC_SET_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_ACCOUNT_SETTING_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteAccountSetting(AccountSetting entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_ACCOUNT_SETTING_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetAccountSetting(AccountSetting entity, string authParms)
        {
            string query = $"SELECT setting.ACC_ACCOUNT_CODE, setting.ACC_GSD_SYS_ID, setting.ACC_SET_SYS_ID, setting.ACC_SET_V_CODE, " +
               // $"act.V_DESC_NAME_AR ACTIVITY_NAME_AR, act.V_DESC_NAME_EN ACTIVITY_NAME_EN, " +
                $" gen.GSD_NAME_AR ACCOUNT_SETTING_AR, gen.GSD_NAME_EN ACCOUNT_SETTING_EN, " +
                $"accounts.ACC_NO, accounts.ACC_NAME_AR, accounts.ACC_NAME_EN " +
                $"FROM FINS_ACCOUNT_SETTING setting " +
                //$"JOIN ACTV_PHRM_VIEW act ON FUN_GET_PARENT_V_CODE(setting.ACC_SET_V_CODE) = FUN_GET_PARENT_V_CODE(act.V_CODE) " +
                $"JOIN GAS_GNRL_SET_DTL gen ON gen.GSD_SYS_ID = setting.ACC_GSD_SYS_ID AND gen.GSD_GSH_SYS_ID = 1 " +
                $"JOIN FINS_ACCOUNT accounts ON accounts.ACC_CODE = setting.ACC_ACCOUNT_CODE " +
                $"WHERE setting.ACC_SET_V_CODE = :pCode";
            var parms = new List<OracleParameter>() { new OracleParameter("pCode", entity.ACC_SET_V_CODE) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetActivityViewData(string authParms)
        {
            var query = $"SELECT DISTINCT PARENT_V_CODE AS V_CODE, PARENT_TYPE, " +
                $" DECODE(PARENT_TYPE, 'OW', V_BR_NAME_AR, 'CM', V_COMP_NAME_AR || ' - ' || V_BR_NAME_AR) AS V_DESC_NAME_AR, " +
                $" DECODE(PARENT_TYPE, 'OW', V_BR_NAME_EN, 'CM', V_COMP_NAME_EN || ' - ' || V_BR_NAME_EN) AS V_DESC_NAME_EN " +
                $"FROM USR_ACTV_VIEW WHERE V_USR_CODE = :pCode AND PARENT_V_CODE IS NOT NULL";
            var parms = new List<OracleParameter>() { new OracleParameter("pCode", OracleDQ.GetAuthenticatedUserObject(authParms).UserCode) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
    }
}
