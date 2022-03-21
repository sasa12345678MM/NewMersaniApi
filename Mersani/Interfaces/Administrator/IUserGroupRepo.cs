using Mersani.models.Administrator;
using System.Collections.Generic;

namespace Mersani.Interfaces.Administrator
{
    public interface IUserGroupRepo
    {
        List<UserGroup> GetUserGroup(int id, string authParms);
        bool PostNewUserGroup(UserGroup userGroup, string authParms);
        bool DeleteUserGroup(int id, string authParms);
    }
}
