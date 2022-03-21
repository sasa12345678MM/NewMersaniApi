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
    public class HrNewHrDepartmentRepository : HrNewHrDepartmentRepo
    {

        public async Task<DataSet> GetHrNewHrDepartmentData(int id, string authParms)
        {
            var query = $"select * from MIRSANIDEV.HR_DEPARTMENTS where (HR_DEPARTMENTS.HRD_SYS_ID= {id} or {id} =0)";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostHrNewHrDepartmentData(List<HrNewHrDepartment> entities, string authParms)
        {
            var authparm = OracleDQ.GetAuthenticatedUserObject(authParms);
            foreach (var entity in entities)
            {
                entity.CURR_USER = (int)authparm?.UserCode;
                if (entity.HRD_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
            }
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_DEPARTMENTS_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteNewHrDepartmentData(HrNewHrDepartment entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_DEPARTMENTS_XML", new List<dynamic>() { entity }, authParms);
        }

       
    }
}
