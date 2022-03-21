using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using System.Data;
using Mersani.Oracle;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;
using System.Linq;

namespace Mersani.Repositories.Adminstrator
{
    public class ActivityRepository: IActivityRepo
    {
        public async Task<DataSet> GetActivityDataList(Activity entity, string authParms)
        {
            var query = "SELECT * FROM GAS_ACTIVITY_MASTER CURR WHERE FAC_CODE = :pFAC_CODE OR :pFAC_CODE = 0";
            var parms = new List<OracleParameter>() { new OracleParameter("pFAC_CODE", entity.FAC_CODE) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkInsertUpdateActivity(List<Activity> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                if (entity.FAC_NAME_AR.Contains("&")) entity.FAC_NAME_AR = entity.FAC_NAME_AR.Replace("&", "&amp;");
                if (entity.FAC_CODE > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_ACTIVITY_MASTER_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteActivityData(Activity entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_ACTIVITY_MASTER_XML", new List<dynamic>() { entity }, authParms);
        }

    }
}
