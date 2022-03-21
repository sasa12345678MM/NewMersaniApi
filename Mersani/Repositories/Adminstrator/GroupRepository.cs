using System;
using Mersani.Oracle;
using System.Collections.Generic;
using Mersani.models.Administrator;
using Mersani.Interfaces.Administrator;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;
using System.Linq;

namespace Mersani.Repositories.Adminstrator
{
    public class GroupRepository : IGroupRepo
    {
        public async Task<DataSet> GetGroup(Group entity, string authParms)
        {
            var query = $"SELECT GR.* FROM GAS_GROUP GR WHERE GR.GROUP_SYS_ID = :pGROUP_SYS_ID OR :pGROUP_SYS_ID = 0";
            var parms = new List<OracleParameter>() { new OracleParameter("pGROUP_SYS_ID", entity.GROUP_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> BulkGroups(List<Group> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                if (entity.GROUP_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_GROUP_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeleteGroup(Group entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_GROUP_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX (TO_NUMBER (GROUP_ID)), 0) + 1 AS Code FROM GAS_GROUP";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
    }
}
