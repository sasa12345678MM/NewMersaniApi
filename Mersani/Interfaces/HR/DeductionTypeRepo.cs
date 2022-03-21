using Mersani.models.HR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.HR
{
    public interface DeductionTypeRepo
    {
        Task<DataSet> GetHrDeductionTypeData(int deductionType, string authParms);
        Task<DataSet> PostHrDeductionTypeData(List<DeductionType> deductionType, string authParms);
        Task<DataSet> DeleteHrDeductionTypeData(DeductionType deductionType, string authParms);
    }
}
