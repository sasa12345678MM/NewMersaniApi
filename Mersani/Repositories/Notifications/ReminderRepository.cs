using Mersani.Interfaces.Notifications;
using Mersani.models.Hubs;
using Mersani.models.Notifications;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Notifications
{
    public class ReminderRepository : IReminderRepo
    {
        // headers
        public async Task<DataSet> GetReminderHeaders(ReminderHeader header, string authParms)
        {
            var query = $"SELECT * FROM REMINDER_HEAD WHERE RH_SYS_ID = :pRH_SYS_ID OR :pRH_SYS_ID = 0";
            var parms = new List<OracleParameter>() { new OracleParameter("pRH_SYS_ID", header.RH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkReminderHeaders(List<ReminderHeader> headers, string authParms)
        {
            foreach (var entity in headers)
            {
                if (entity.RH_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_REMINDER_HEAD_XML", headers.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteReminderHeader(ReminderHeader header, string authParms)
        {
            header.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_REMINDER_HEAD_XML", new List<dynamic>() { header }, authParms);
        }


        // details
        public async Task<DataSet> GetReminderDetails(ReminderDetail detail, string authParms)
        {
            var query = $"SELECT REMINDER_DETAIL.*, GAS_MNU.MNU_LABEL_AR, GAS_MNU.MNU_LABEL_EN FROM REMINDER_DETAIL LEFT JOIN GAS_MNU ON MNU_CODE = RU_MNU_CODE WHERE RD_RH_SYS_ID = :pRD_RH_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pRD_RH_SYS_ID", detail.RD_RH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkReminderDetails(List<ReminderDetail> details, string authParms)
        {
            foreach (var entity in details)
            {
                if (entity.RD_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_REMINDER_DETAIL_XML", details.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteReminderDetail(ReminderDetail detail, string authParms)
        {
            detail.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_REMINDER_DETAIL_XML", new List<dynamic>() { detail }, authParms);
        }


        // users
        public async Task<DataSet> GetReminderUsers(ReminderUser user, string authParms)
        {
            var query = $"SELECT RU_SYS_ID, RU_RD_SYS_ID, RU_USR_ROLE_SYS_ID, RU_LEVEL, RU_BEF_DAYS, RU_AFT_DAYS, GUR_DESC_AR, GUR_DESC_EN FROM REMINDER_USER, GAS_USR_ROLE " +
                $" WHERE RU_USR_ROLE_SYS_ID = GUR_SYS_ID AND RU_RD_SYS_ID = :pRU_RD_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pRU_RD_SYS_ID", user.RU_RD_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkReminderUsers(List<ReminderUser> users, string authParms)
        {
            foreach (var entity in users)
            {
                if (entity.RU_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_REMINDER_USER_XML", users.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteReminderUser(ReminderUser user, string authParms)
        {
            user.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_REMINDER_USER_XML", new List<dynamic>() { user }, authParms);
        }

        public async Task<DataSet> GetNotifications(ReminderUser user, string authParms)
        {
            var query = $"SELECT V_REM.*, MNU_PATH FROM V_REM, GAS_MNU WHERE RU_MNU_CODE = MNU_CODE AND V_USR_CODE = :pUSR_CODE AND TXN_V_CODE = :pRU_V_CODE";
            var parms = new List<OracleParameter>() { 
                new OracleParameter("pUSR_CODE", user.RU_USR_CODE),
                new OracleParameter("pRU_V_CODE", user.RU_V_CODE)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public List<ReminderModel> GetReminders(ReminderUser user, string authParms)
        {
            var query = $"SELECT V_REM.*, MNU_PATH FROM V_REM, GAS_MNU WHERE RU_MNU_CODE = MNU_CODE AND V_USR_CODE = :pUSR_CODE AND TXN_V_CODE = :pRU_V_CODE";
            return OracleDQ.GetData<ReminderModel>(query,authParms, new { pUSR_CODE= user.RU_USR_CODE, pRU_V_CODE= user.RU_V_CODE }, CommandType.Text);
        }
    }
}
