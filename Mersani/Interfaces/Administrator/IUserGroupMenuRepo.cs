using System.Collections.Generic;
using Mersani.models.Administrator;

namespace Mersani.Interfaces.Administrator
{
    public interface IUserGroupMenuRepo
    {
        List<UserGroupMenu> GetUserGroupMenu(int id, string authParms);
        List<UserGroupMenu> GetMenuesByUserGroup(int userGroupId, string authParms);
        bool PostNewUserGroupMenu(UserGroupMenu userGroupMenu, string authParms);
        bool DeleteUserGroupMenu(int id, string authParms);
    }
}
