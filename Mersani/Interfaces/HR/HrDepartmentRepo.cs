using Mersani.models.HR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.HR
{
    public interface HrDepartmentRepo
    {
        Task<DataSet> GetHrDepartmentData( int sysId ,string authParms);
        Task<DataSet> PostHrDepartmentData(List<HrDepartment> entities, string authParms);
        Task<DataSet> DeleteHrDepartmentData(HrDepartment entity ,string authParms);
        Task<DataSet> GetLastCode(string authParms);
    }
}
