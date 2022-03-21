using Mersani.Controllers.Administrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
    public interface GeneralSharedRepo
    {
        Task<DataSet> getInvItemcurrStk(int invSysId, int itemSysId, dynamic batchSysId, dynamic uomSysId, string authParms);
        Task<DataSet> GetLoggedInPharmacyData(string authParms);

        Task<DataSet> GetNearbyPharmacies(NearbyPharmaciesPosition position);
    }
}
