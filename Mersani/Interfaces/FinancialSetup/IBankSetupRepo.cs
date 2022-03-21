using Mersani.models.FinancialSetup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.FinancialSetup
{
    public interface IBankSetupRepo
    {
        Task<DataSet> GetBankSetup(int id,string authParms);
        Task<bool> PostBankSetup(BankSetup bank, string authParms);
        Task<bool> DeletBankSetup(int id, string authParms);

    }
}
