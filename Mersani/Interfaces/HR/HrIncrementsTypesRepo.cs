using Mersani.models.HR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.HR
{
   public interface HrIncrementsTypesRepo
    {
        Task<DataSet> GetHrIncrementsTypesData(int hrIncrementsTypes, string authParms);
        Task<DataSet> PostHrIncrementsTypesData(List<HrIncrementsTypes> hrIncrementsTypes, string authParms);
        Task<DataSet> DeleteHrIncrementsTypesData(HrIncrementsTypes hrIncrementsTypes, string authParms);
    }
}
