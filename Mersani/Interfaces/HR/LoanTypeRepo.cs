using Mersani.models.HR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.HR
{
    public interface LoanTypeRepo
    {

        Task<DataSet> GetHrLoansTypeData(int hrLoansType, string authParms);
        Task<DataSet> PostHrLoansTypeData(List<HrLoansType> hrLoansType, string authParms);
        Task<DataSet> DeleteHrLoansTypeData(HrLoansType lrLoansType, string authParms);


    }
}
