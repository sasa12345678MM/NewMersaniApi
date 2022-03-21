using Mersani.models.HR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.HR
{
   public interface HrExpensestypesRepo
    {
        Task<DataSet> GetHrExpensestypesData(int hrExpensestypes, string authParms);
        Task<DataSet> PostHrExpensestypesData(List<HrExpensestypes> hrExpensestypes, string authParms);
        Task<DataSet> DeleteHrExpensestypesData(HrExpensestypes hrExpensestypes, string authParms);
    }
}
