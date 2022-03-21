using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mersani.models.FinancialSetup;

namespace Mersani.Interfaces.FinancialSetup
{
    public interface IAccountSettingRepo
    {
        Task<DataSet> BulkInsertUpdateAccountSetting(List<AccountSetting> accountSetting, string authParms);
        Task<DataSet> DeleteAccountSetting(AccountSetting accountSetting, string authParms);
        Task<DataSet> GetAccountSetting(AccountSetting accountSetting, string authParms);

        Task<DataSet> GetActivityViewData(string authParms);
    }
}
