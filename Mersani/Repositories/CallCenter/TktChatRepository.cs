using Mersani.Interfaces.CallCenter;
using Mersani.models.Hubs;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Repositories.CallCenter
{
    public class TktChatRepository : ITktChatRepo
    {
        public async Task<DataSet> GetChatHistory(TktChat chat, string authParms)
        {
            var query = $"SELECT V_TKT_CHAT.*, (CASE WHEN TC_SENDER = '{chat.TC_SENDER}' THEN 'S' ELSE 'R' END) AS TC_TYPE " +
                $" FROM V_TKT_CHAT WHERE TC_SENDER IN ('{chat.TC_SENDER}', '{chat.TC_RECEIVER}') AND TC_RECEIVER IN ('{chat.TC_SENDER}', '{chat.TC_RECEIVER}') ORDER BY TC_DATE";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text, _public: true);
        }
        public async Task<DataSet> GetChatHistoryForCustomer(string sender, string authParms)
        {
            var query = $"SELECT V_TKT_CHAT.*, (CASE WHEN TC_SENDER = '{sender}' THEN 'S' ELSE 'R' END) AS TC_TYPE " +
                $" FROM V_TKT_CHAT WHERE TC_SENDER IN ('{sender}') OR TC_RECEIVER IN ('{sender}') ORDER BY TC_DATE";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text, _public: true);
        }


        public async Task<DataSet> GetChatRecieversHistory(TktChat chat, string authParms)
        {
            var query = $"SELECT DISTINCT * FROM(" +
                $" SELECT TO_NUMBER(CHT.TC_RECEIVER) AS USR_CODE, CHT.RECEIVER_USR_LOGIN AS USR_LOGIN, CHT.RECEIVER_NAME_AR AS USR_NAME_AR, CHT.RECEIVER_NAME_EN AS USR_NAME_EN, 0 AS UNREADMSG, 0 AS ACTIVE " +
                $" FROM V_TKT_CHAT CHT WHERE TC_SENDER = TO_CHAR(:pSENDER_ID) " +
                $" UNION ALL " +
                $" SELECT TO_NUMBER(CHT.TC_SENDER) AS USR_CODE, CHT.SENDER_USR_LOGIN AS USR_LOGIN, CHT.SENDER_NAME_AR AS USR_NAME_AR, CHT.SENDER_NAME_EN AS USR_NAME_EN, 0 AS UNREADMSG, 0 AS ACTIVE " +
                $" FROM V_TKT_CHAT CHT WHERE TC_RECEIVER = TO_CHAR(:pSENDER_ID)) ";
            var parms = new List<OracleParameter>() { new OracleParameter("pSENDER_ID", chat.TC_SENDER) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text, _public: true);
        }
    }
}
