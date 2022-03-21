using Mersani.models.Administrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
    public interface IUserRolesRepo
    {


        Task<DataSet> getUserRoles(UserRoles entity, string authParms);
        Task<DataSet> bulkUserRoles(List<UserRoles> entities, string authParms);
        Task<DataSet> deleteUserRoles(UserRoles entity, string authParms);

    }


}     
