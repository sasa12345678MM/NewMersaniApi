using System.Data;
using Mersani.Oracle;
using Mersani.models.Users;
using System.Threading.Tasks;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

namespace Mersani.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        public async Task<bool> UploadProfileImg(UserData user, string authParms)
        {
            var query = $"UPDATE GAS_USR SET PIC_PATH = :pPIC_PATH WHERE USR_CODE = :pUSR_CODE OR :pUSR_CODE = 0";
            return await OracleDQ.PostDataAsync(query, authParms, new { pPIC_PATH = user.PIC_PATH, pUSR_CODE = user.USR_CODE });


        }

        public async Task<DataSet> UpdateUserProfileData(UserData user, string authParms)
        {
            var query = $"UPDATE GAS_USR " +
                $"SET PIC_PATH = :pPIC_PATH, USR_LOGIN = :pUSR_LOGIN, USR_PW = :pUSR_PW, USR_FULL_NAME_AR = :pUSR_FULL_NAME_AR, " +
                $"USR_FULL_NAME_EN = :pUSR_FULL_NAME_EN, USR_MOB = :pUSR_MOB, USR_EMAIL_ID = :pUSR_EMAIL_ID, USR_TEL = :pUSR_TEL " +
                $"WHERE USR_CODE = :pUSR_CODE OR :pUSR_CODE = 0";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                    new OracleParameter("pPIC_PATH", user.PIC_PATH),
                    new OracleParameter("pUSR_LOGIN", user.USR_LOGIN),
                    new OracleParameter("pUSR_PW", user.USR_PW),
                    new OracleParameter("pUSR_FULL_NAME_AR", user.USR_FULL_NAME_AR),
                    new OracleParameter("pUSR_FULL_NAME_EN", user.USR_FULL_NAME_EN),
                    new OracleParameter("pUSR_MOB", user.USR_MOB),
                    new OracleParameter("pUSR_EMAIL_ID", user.USR_EMAIL_ID),
                    new OracleParameter("pUSR_TEL", user.USR_TEL),
                    new OracleParameter("pUSR_CODE", user.USR_CODE),
                }, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetUserData(int UserCode, string authParms)
        {
            var query = $"SELECT * FROM GAS_USR WHERE USR_CODE = :pUSR_CODE OR :pUSR_CODE = 0";
            var parms = new List<OracleParameter>() { new OracleParameter("pUSR_CODE", UserCode) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostUserData(UserData user, string authParms)
        {
            if (user.USR_CODE > 0) user.STATE = (int)OperationType.Update;
            else user.STATE = (int)OperationType.Add;
            user.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_USR_XML", new List<dynamic>() { user }, authParms);
        }

        public async Task<DataSet> DeleteUserData(UserData user, string authParms)
        {
            user.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_USR_XML", new List<dynamic>() { user }, authParms);
        }

        public List<UpladFile> GetEncryptedFileName( string authParms)
        {
            var query = $"select  FUN_GEN_ATTNAME as FileName from dual";
            List<UpladFile> UpladFile = OracleDQ.GetData<UpladFile>(query, authParms, null);
            return UpladFile;
        }
    }
}
