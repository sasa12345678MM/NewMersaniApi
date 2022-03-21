using Mersani.models.HR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.HR
{
    public interface HrJobGroupsRepo
    {
        Task<DataSet> GetHrJobGroupsData(int hrJobGroups, string authParms);
        Task<DataSet> PostHrJobGroupsData(List<HrJobGroups> hrJobGroups, string authParms);
        Task<DataSet> DeleteHrJobGroupsData(HrJobGroups hrJobGroups, string authParms);
    }
}
