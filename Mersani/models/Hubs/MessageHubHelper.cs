using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.models.Hubs
{
    public class MessageHubHelper
    {
        public async Task<DataSet> saveMessage(TktChat message, string authParms)
        {
            return await OracleDQ.ExcuteXmlProcAsync("PRC_TKT_CHAT_SAVE", new List<dynamic>() { message }, "", true);
        }

        public DataSet ConnectUser(string userId, string connectionId, string authParms)
        {
            //if exist update ConnectionId , LastActiveTime , IsActive = true
            //string encodedParamters = SerlizeInputXml.Encode(new List<dynamic>() { new NotifyActiveUser() {

            //     Serial = -1,
            //     ConnectionId = ConnectionId,
            //     IsActive = true,
            //     LastActiveTime =  DateTime.Now,
            //     SecLoginId = userId

            //} });

            //var result = NotifyAccessor.Repo.NotifyActiveUserSave(encodedParamters, secParms);
            return null;// result;

        }

        public DataSet DisConnectUser(string userId, string ConnectionId, string secParms)
        {
            //string encodedParamters = SerlizeInputXml.Encode(new List<dynamic>() { new NotifyActiveUser() {

            //     Serial = 1,
            //     ConnectionId = ConnectionId,
            //     IsActive = false,
            //     LastActiveTime =  DateTime.Now,
            //     SecLoginId = userId

            //} });

            //var result = NotifyAccessor.Repo.NotifyActiveUserSave(encodedParamters, secParms);
            return null;// result;
        }

        public Task<DataSet> GetUserDetail(string userId, string authParam, string userType = "U")
        {
            string query = userType == "C" ?
                $"select CUST_SYS_ID USR_CODE, CUST_NAME_AR USR_FULL_NAME_AR,CUST_NAME_EN USR_FULL_NAME_EN, CUST_ATT_MOBILE USR_LOGIN from fins_customer where CUST_SYS_ID = :pUSR_CODE"
                : $"SELECT* FROM GAS_USR WHERE USR_CODE = :pUSR_CODE";
            return OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() { new OracleParameter("pUSR_CODE", userId) }, authParam, CommandType.Text);
        }
        public DataSet GetChat(string Sender, string Reciver, string secParms)
        {
            //var res = NotifyAccessor.Repo.Chatload(Sender, Reciver, secParms);
            return null;// res;
        }

    }
}
