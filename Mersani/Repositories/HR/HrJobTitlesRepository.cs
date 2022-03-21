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
    public class HrJobTitlesRepository : HrJobTitlesRepo
    {
        public async Task<DataSet> GetHrJobTitlesData(int hrJobTitles, string authParms)
        {
            var query = $"SELECT * FROM MIRSANIDEV.HR_JOBS_TITLES WHERE (MIRSANIDEV.HR_JOBS_TITLES.hrjt_sys_id= {hrJobTitles} or {hrJobTitles} =0)";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        //public async Task<DataSet> PostHrJobTitlesData(List<HrJobTitles> entities, string authParms)
        //{
        //    var authparm = OracleDQ.GetAuthenticatedUserObject(authParms);
        //    foreach (var entity in entities)
        //    {
        //        entity.CURR_USER = (int)authparm?.UserCode;
        //        if (entity.HRJT_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
        //        else entity.STATE = (int)OperationType.Add;
        //    }
        //    return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_JOBS_TITL_XML", entities.ToList<dynamic>(), authParms);
        //}

        public async Task<DataSet> PostHrJobTitlesData(List<HrJobTitles> entities, string authParms)
        {
            foreach (var HrJobTitles in entities)
            {

                if (HrJobTitles.HRJT_SYS_ID > 0) HrJobTitles.STATE = (int)OperationType.Update;
                else HrJobTitles.STATE = (int)OperationType.Add;
                HrJobTitles.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_JOBS_TITL_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteHrJobTitlesData(HrJobTitles entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_JOBS_TITL_XML", new List<dynamic>() { entity }, authParms);
        }
    }
}
