using Mersani.models.HR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.HR
{
    public interface CostCenterRepo
    {
        Task<DataSet> GetHrCostCenterData(int CostCentar, string authParms);
        Task<DataSet> PostHrCostCenterData(List<CostCentar> costCentar, string authParms);
        Task<DataSet> DeleteHrCostCenterData(CostCentar costCentar, string authParms);
    }
}
