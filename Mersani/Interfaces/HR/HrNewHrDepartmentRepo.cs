using Mersani.models.HR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.HR
{
   public interface HrNewHrDepartmentRepo
    {
        Task<DataSet> GetHrNewHrDepartmentData(int id, string authParms);
        Task<DataSet> PostHrNewHrDepartmentData(List<HrNewHrDepartment> entities, string authParms);
        Task<DataSet> DeleteNewHrDepartmentData(HrNewHrDepartment entity, string authParms);

    }
}
