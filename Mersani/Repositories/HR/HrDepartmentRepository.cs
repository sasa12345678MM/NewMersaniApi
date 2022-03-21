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
    public class HrDepartmentRepository : HrDepartmentRepo
    {
        public async Task<DataSet> GetHrDepartmentData( int sysId,string authParms)
        {
            var query = $"select * from HR.HR_DEPARTMENT where (HR.HR_DEPARTMENT.HRD_SYS_ID= {sysId} or {sysId} =0)";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
        public async Task<DataSet> PostHrDepartmentData(List<HrDepartment> entities, string authParms)
        {
            var authparm = OracleDQ.GetAuthenticatedUserObject(authParms);
            foreach (var entity in entities)
            {
                entity.CURR_USER = (int)authparm?.UserCode;
                if (entity.HRD_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
            }
              return await OracleDQ.ExcuteXmlProcAsync("HR.PRC_HR_DEPARTMENT_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeleteHrDepartmentData(HrDepartment entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("HR.PRC_HR_DEPARTMENT_XML", new List<dynamic>() { entity }, authParms);
        }
        public Task<DataSet> GetLastCode(string authParms)
        {
            throw new NotImplementedException();
        }
     
    }
}
