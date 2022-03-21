using Mersani.models.PointOfSale;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.PointOfSale
{
    public interface IInsuranceContractRepo
    {
        Task<DataSet> GetInsuranceContractDataList(InsuranceContract entity, string authParms);
        Task<DataSet> InsertUpdateInsuranceContract(InsuranceContract entity, string authParms);
        Task<DataSet> DeleteInsuranceContract(InsuranceContract entity, string authParms);

        Task<DataSet> GetContractLastCode(string authParms);


        Task<DataSet> GetInsuranceContractClassList(InsuranceContractClass entity, string authParms);
        Task<DataSet> GetInsuranceContractClassById(InsuranceContractClass entity, string authParms);
        Task<DataSet> PostInsuranceContractClasses(InsuranceContractClass entity, string authParms);
        Task<DataSet> DeleteInsuranceContractClasses(InsuranceContractClass entity, string authParms);
    }
}
