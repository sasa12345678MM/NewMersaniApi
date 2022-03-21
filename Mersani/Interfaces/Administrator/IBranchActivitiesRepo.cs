using Mersani.models.Administrator;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
    public interface IBranchActivitiesRepo
    {
        Task<DataSet> GetBranchActivities(BranchActivities entity, string authParms);
        Task<DataSet> GetActivityByBranch(BranchActivities entity, string authParms);
        Task<DataSet> BulkBranchActivities(List<BranchActivities> entities, string authParms);
        Task<DataSet> DeleteBranchActivity(BranchActivities entity, string authParms);
    }
}
