using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mersani.models.Administrator;

namespace Mersani.Interfaces.Administrator
{
    public interface ICompanyBranchesRepo
    {
        Task<DataSet> GetBranch(CompanyBranches entity, string authParms);
        Task<DataSet> GetBranchByCompany(CompanyBranches entity, string authParms);
        Task<DataSet> BulkBranches(List<CompanyBranches> entities, string authParms);
        Task<DataSet> DeleteBranch(CompanyBranches entity, string authParms);

        Task<DataSet> GetLastCode(int id, string authParms);
    }
}
