using Mersani.models.PointOfSale;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.PointOfSale
{
    public interface IInsuranceCompanyRepo
    {
        Task<DataSet> GetInsuranceCompanyDataList(InsuranceCompany entity, string authParms);
        Task<DataSet> InsertUpdateInsuranceCompany(InsuranceCompany entity, string authParms);
        Task<DataSet> DeleteInsuranceCompany(InsuranceCompany entity, string authParms);

        Task<DataSet> GetLastCode(string authParms);
    }
}
