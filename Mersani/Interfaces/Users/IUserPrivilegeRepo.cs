using Mersani.models.Users;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Users
{
    public interface IUserPrivilegeRepo
    {
        //Task<List<UserPrivilege>> GetUserPrivilege(int id, string authParms);
        //Task<List<UserPrivilege>> GetUserPrivilegeByActivity(int userActivityId, string authParms);
        Task<List<UserPrivilege>> CheckUserPrivilegeByUrl(UserPrivilege userPrivilege, string authParms);
        //Task<bool> PostNewUserPrivilege(UserPrivilege userComBrPrivilege, string authParms);
        //Task<bool> DeleteUserPrivilege(int id, string authParms);

        // new code
        Task<DataSet> BulkUserPrivileges(List<UserPrivilege> privileges, string authParms);
        Task<DataSet> GetUserPrivilegesByActivity(UserPrivilege privilege, string authParms);
        Task<DataSet> GetUserPrivilegeById(UserPrivilege privilege, string authParms);
        Task<DataSet> DeleteUserPrivilege(UserPrivilege privilege, string authParms);
    }
}
