using Mersani.models.HR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.HR
{
   public interface HrJobRepo
    {
        Task<DataSet> GetHrJobData(int HrJob, string authParms);
        Task<DataSet> PostHrJobData(List<HrJob> HrJob, string authParms);
        Task<DataSet> DeleteHrJobData(HrJob HrJob, string authParms);
    }
}
