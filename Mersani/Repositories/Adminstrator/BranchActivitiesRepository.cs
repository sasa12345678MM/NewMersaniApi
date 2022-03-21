using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class BranchActivitiesRepository : IBranchActivitiesRepo
    {
        public async Task<DataSet> GetActivityByBranch(BranchActivities entity, string authParms)
        {
            var query = $"SELECT FACB.FAC_SYS_ID, FACB.FAC_BR_SYS_ID, FACB.FAC_ACTIVITY_CODE, CB.CB_NAME_AR AS BRANCH_NAME_AR, CB.CB_NAME_EN AS BRANCH_NAME_EN," +
                        $" ACT.FAC_NAME_AR AS ACTIVITY_NAME_AR, ACT.FAC_NAME_EN AS ACTIVITY_NAME_EN FROM GAS_BR_ACTV FACB" +
                        $" LEFT OUTER JOIN GAS_COMPANY_BRANCHES CB ON CB.CB_SYS_ID = FACB.FAC_BR_SYS_ID" +
                        $" LEFT OUTER JOIN GAS_ACTIVITY_MASTER ACT ON ACT.FAC_CODE = FACB.FAC_ACTIVITY_CODE" +
                        $" WHERE FACB.FAC_BR_SYS_ID = :pFAC_BR_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pFAC_BR_SYS_ID", entity.FAC_BR_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetBranchActivities(BranchActivities entity, string authParms)
        {
            var query = $"SELECT FACB.FAC_SYS_ID, FACB.FAC_BR_SYS_ID, FACB.FAC_ACTIVITY_CODE, CB.CB_NAME_AR AS BRANCH_NAME_AR, CB.CB_NAME_EN AS BRANCH_NAME_EN," +
                        $" ACT.FAC_NAME_AR AS ACTIVITY_NAME_AR, ACT.FAC_NAME_EN AS ACTIVITY_NAME_EN FROM GAS_BR_ACTV FACB" +
                        $" LEFT OUTER JOIN GAS_COMPANY_BRANCHES CB ON CB.CB_SYS_ID = FACB.FAC_BR_SYS_ID" +
                        $" LEFT OUTER JOIN GAS_ACTIVITY_MASTER ACT ON ACT.FAC_CODE = FACB.FAC_ACTIVITY_CODE" +
                        $" WHERE FACB.FAC_SYS_ID = :pFAC_SYS_ID OR :pFAC_SYS_ID = 0";
            var parms = new List<OracleParameter>() { new OracleParameter("pFAC_SYS_ID", entity.FAC_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> BulkBranchActivities(List<BranchActivities> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                if (entity.FAC_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_BR_ACTV_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteBranchActivity(BranchActivities entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_BR_ACTV_XML", new List<dynamic>() { entity }, authParms);
        }
    }
}
