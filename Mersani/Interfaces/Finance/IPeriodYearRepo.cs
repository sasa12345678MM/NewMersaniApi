using Mersani.models.Finance;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Finance
{
    public interface IPeriodYearRepo
    {
        // new
        Task<DataSet> GetFinancialYear(FinsYear entity, string authParms);
        Task<DataSet> GetFinancialPeriods(FinsYear entity, string authParms);
        Task<DataSet> PostFinancialYearPeriods(FinancialYearPeriods entity, string authParms);
        Task<DataSet> DeleteFinancialYear(FinsYear entity, int type, string authParms);
    }
}



