using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class UserRolesRepository : IUserRolesRepo
    {
        public async Task<DataSet> bulkUserRoles(List<UserRoles> entities, string authParms)
        {
            foreach (UserRoles entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.GUR_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_User_Roles_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> deleteUserRoles(UserRoles entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_User_Roles_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> getUserRoles(UserRoles entity, string authParms)
        {
            var query = $"SELECT * FROM GAS_USR_ROLE WHERE GUR_SYS_ID = :pGUR_SYS_ID or :pGUR_SYS_ID=0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pGUR_SYS_ID", entity.GUR_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
    }
}
