using Mersani.models.FinancialSetup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.FinancialSetup
{
    public interface IFinsCostCeneterRepository
    {
        Task<DataSet> GetFinCostCenterDetails(FinsCostCeneter entity, string authParms);

        Task<DataSet> PostFinCostCenter(List<FinsCostCeneter> FinisCostCenter, string authParms);
        Task<DataSet> DeleteFinCostCenter(FinsCostCeneter entity, string authParms);
       
    }
}
