using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mersani.models.Administrator;

namespace Mersani.Interfaces.Administrator
{
    public interface ICompanyRepo
    {
        Task<DataSet> GetCompany(Company entity, string authParms);
        Task<DataSet> GetCompaniesByGroup(Company entity, string authParms);
        Task<DataSet> BulkCompanys(List<Company> entities, string authParms);
        Task<DataSet> DeleteCompany(Company entity, string authParms);

        Task<DataSet> GetLastCode(int id, string authParms);
    }
}
