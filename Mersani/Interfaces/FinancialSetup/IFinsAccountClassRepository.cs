using Mersani.models.FinancialSetup;
using System.Collections.Generic;

namespace Mersani.Interfaces.FinancialSetup
{
    public interface IFinsAccountClassRepository
    {
        List<FinsAcountClass> GetFINS_ACC_CLASSDetails(int id, string authParms);
        bool PostNewFINS_ACC_CLASS(FinsAcountClass finsAccountClass, string authParms);
        bool UpdateFINS_ACC_CLASS(int id, FinsAcountClass finsAccountClass, string authParms);
        bool DeleteFINS_ACC_CLASS(int id, string authparms);
        List<FinsAcountClass> SearchFinsAccountClass(FinsAcountClass criteria, string authParms);
    }
}
