using Mersani.models.HR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.HR
{
    public interface HrSitesRepo
    {

        Task<DataSet> GetHrSitesData(int id, string authParms);
        Task<DataSet> PostHrSitesData(List<HrSites> entities, string authParms);
        Task<DataSet> DeleteHrSitesData(HrSites entity, string authParms);

    }
}
