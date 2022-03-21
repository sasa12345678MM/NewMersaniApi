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
    public class HrSitesRepository : HrSitesRepo
    {
        public async Task<DataSet> DeleteHrSitesData(HrSites entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_SITES_XML", new List<dynamic>() { entity }, authParms);
        }
        
        public async Task<DataSet> GetHrSitesData(int hrSites, string authParms)
        {
            var query = $"select * from MIRSANIDEV.HR_SITES where (HR_SITES.HRS_SYS_ID= {hrSites} or {hrSites} =0)";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostHrSitesData(List<HrSites> entities, string authParms)
        {
            var authparm = OracleDQ.GetAuthenticatedUserObject(authParms);
            foreach (var entity in entities)
            {
                entity.CURR_USER = (int)authparm?.UserCode;
                if (entity.HRS_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
            }
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_SITES_XML", entities.ToList<dynamic>(), authParms);
        }
    }
}
