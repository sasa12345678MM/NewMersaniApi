using Mersani.Interfaces.Auth;
using Mersani.models.Auth;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        public async Task<DataSet> GetActivityView(string userActivityCode, int userCode, string authParms)
        {
            return await OracleDQ.ExcuteGetQueryAsync("SELECT * FROM USR_ACTV_VIEW WHERE V_CODE = :pUserActCode AND V_USR_CODE = :pUserCode", new List<OracleParameter>() {
                new OracleParameter("pUserActCode", userActivityCode),
                new OracleParameter("pUserCode", userCode)
            }, authParms, CommandType.Text);
        }

        public async Task<DataSet> Login(UserLoginModal user)
        {
            return await OracleDQ.LoginAuthCheck("LOGIN_AUTH_CHECK", user);
        }
    }
}
