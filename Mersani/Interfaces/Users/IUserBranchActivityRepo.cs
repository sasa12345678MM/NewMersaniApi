using Mersani.models.Users;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Users
{
    public interface IUserBranchActivityRepo
    {
        Task<List<UserBranchActivity>> GetUserBranchActivity(int id, string authParms);
        Task<bool> PostNewUserBranchActivity(UserBranchActivity userCompanyBranch, string authParms);
        Task<bool> DeleteUserBranchActivity(int id, string authParms);
        Task GetpharmcyActivity(int id, string authParms);
        //Task<DataSet> BulkUserBranchActivity(List<UserBranchActivity> pharmacies, string authParms);
        //Task<DataSet> DeleteUserBranchActivityxml(int id, string authParms);
    }
}
