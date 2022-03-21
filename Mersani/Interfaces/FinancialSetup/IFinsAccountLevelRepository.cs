using Mersani.models.FinancialSetup;
using System.Collections.Generic;

namespace Mersani.Interfaces.FinancialSetup
{
    public interface IFinsAccountLevelRepository
    {
        List<FinsAccountLevel> GetFinAccountLevelDetails(int id, string authParms);
        bool PostNewFinAccountLevel(FinsAccountLevel finsAccountLevel, string authParms);
        bool UpdateFinAccountLevel(int id, FinsAccountLevel finsAccountLevel, string authParms);
        bool DeleteFinAccountLevel(int id, string authparms);
        List<FinsAccountLevel> SearchFinsAccountLevel(FinsAcountClass criteria, string authParms);

    }
}
