using Mersani.Interfaces.HR;
using Mersani.models.HR;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.HR
{
    public class HrJobRepository : HrJobRepo
    {
        public async Task<DataSet> GetHrJobData(int  sys_id, string authParms)
        {
   
            var query = $"SELECT * FROM  HR.HR_JOB WHERE HRJ_SYS_ID = {sys_id} OR {sys_id} = 0";
         
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);

        }

        public async Task<DataSet> PostHrJobData(List<HrJob> entities, string authParms)
        {
            foreach(var HrJob in entities){

            if (HrJob.HRJ_SYS_ID > 0) HrJob.STATE = (int)OperationType.Update;
            else HrJob.STATE = (int)OperationType.Add;
            HrJob.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }
            return await OracleDQ.ExcuteXmlProcAsync("HR.PRC_HR_JOB_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeleteHrJobData(HrJob HrJob, string authParms)
        {
            HrJob.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("HR.PRC_HR_JOB_XML", new List<dynamic>() { HrJob }, authParms);
        }


    }
}
