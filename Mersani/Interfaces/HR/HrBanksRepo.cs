using Mersani.models.HR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.HR
{
    public interface HrBanksRepo
    {
        Task<DataSet> GetHrBanksData(int HrBanks, string authParms);
        Task<DataSet> PostHrBanksData(List<HrBanks> HrBanks, string authParms);
        Task<DataSet> DeleteHrBanksData(HrBanks HrBanks, string authParms);
    }
}
