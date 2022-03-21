using Mersani.models.Finance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Finance
{
    public interface AccountStatementRepo
    {
        Task<DataSet> GetAccountStatement(AccountStatement AccountStatement, string authParms);

    }
}
 