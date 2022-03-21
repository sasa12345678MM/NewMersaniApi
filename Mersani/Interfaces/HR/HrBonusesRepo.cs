using Mersani.models.HR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.HR
{
   public interface HrBonusesRepo
    {
        Task<DataSet> GetHrBonusesData(int hrBounses, string authParms);
        Task<DataSet> PostHrBonusesData(List<HrBonuses> hrBounses, string authParms);
        Task<DataSet> DeleteHrBonusesData(HrBonuses hrBounses , string authParms);
    }
}
