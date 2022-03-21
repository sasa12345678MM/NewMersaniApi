using System.Data;
using System.Threading.Tasks;
using Mersani.models.FinancialSetup;

namespace Mersani.Interfaces.FinancialSetup
{
    public interface IFinisAccountRepository
    {
        Task<DataSet> PostFinsAccount(FinsAccount entity, string authParms);
        Task<DataSet> DeletFinsAccount(FinsAccount entity, string authParms);
        Task<DataSet> GetFinsAccount(FinsAccount entity, string authParms);
        Task<DataSet> GetFinsAccountChildern(FinsAccount entity, string authParms);
        Task<DataSet> GetAccountNoTwoByParent(FinsAccount entity, string authParms);
        Task<DataSet> GetAccountNoThreeByParent(FinsAccount entity, string authParms);
    }
}