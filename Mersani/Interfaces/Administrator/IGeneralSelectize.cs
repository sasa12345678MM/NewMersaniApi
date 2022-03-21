using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mersani.models.Administrator;
using Mersani.models.FinancialSetup;

namespace Mersani.Interfaces.Administrator
{
    public interface IGeneralSelectize
    {
        // first
        Task<List<dynamic>> GetUserActivities(int id, string authParms);
        // second
        Task<List<dynamic>> GetBranchesByActivity(int id, string authParms);
        // third
        Task<List<dynamic>> GetCompaniesByBranch(int id, string authParms);
        // other
        List<FinsAccountLevel> GetFinsAccountLevel(int AccountLevelCode, string authParms);
        List<FinsAcountClass> GetFinsAccountClass(int AccountClassCode, string authParms);
        List<FinsCostCeneter> GetFinsCostCenter(int CostCenterCode, string authParms);
        Task<List<dynamic>> getMirsaniSelectData(ISelectSearch selectSearch, string authParms);


        Task<DataSet> GetDynamicDataByAppCode(ISelectSearch selectSearch, string authParms);
    }
}
