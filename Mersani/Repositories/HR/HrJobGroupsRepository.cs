using Mersani.Interfaces.HR;
using Mersani.models.HR;
using Mersani.Oracle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.HR
{
    public class HrJobGroupsRepository : HrJobGroupsRepo
    {
        public async Task<DataSet> GetHrJobGroupsData(int hrJobGroups, string authParms)
        {
            var query = $"SELECT * FROM  MIRSANIDEV.HR_JOBS_GROUPS WHERE HRJG_SYS_ID = {hrJobGroups} OR {hrJobGroups} = 0";

            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
        public async Task<DataSet> PostHrJobGroupsData(List<HrJobGroups> entities, string authParms)
        {
            foreach (var HrJobGroups in entities)
            {

                if (HrJobGroups.HRJG_SYS_ID > 0) HrJobGroups.STATE = (int)OperationType.Update;
                else HrJobGroups.STATE = (int)OperationType.Add;
                HrJobGroups.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_JOBS_GRP_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeleteHrJobGroupsData(HrJobGroups hrJobGroups, string authParms)
        {
            hrJobGroups.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_JOBS_GRP_XML", new List<dynamic>() { hrJobGroups }, authParms);
        }


    }
}
