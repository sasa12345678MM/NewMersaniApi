using Mersani.Interfaces.Finance;
using Mersani.models.Finance;
using Mersani.Oracle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Finance
{
    public class AccountStatementRepository : AccountStatementRepo
    {
        public async Task<DataSet> GetAccountStatement(AccountStatement AccountStatement, string authParms)
        {

            AccountStatement.INS_USER = (int)OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            AccountStatement.V_CODE = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;
            
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_YEARS_XML", new List<dynamic>() {AccountStatement }, authParms);
        }
    }
}
