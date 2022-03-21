using Mersani.Interfaces.HR;
using Mersani.models.HR;
using Mersani.Oracle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.HR
{
    public class HrLoanTypeRepository : LoanTypeRepo
    {
        public async Task<DataSet> DeleteHrLoansTypeData(HrLoansType lrLoansType, string authParms)
        {
            lrLoansType.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_LOANS_TYPE_XML", new List<dynamic>() { lrLoansType }, authParms);
        }

        public async Task<DataSet> GetHrLoansTypeData(int hrLoansType, string authParms)
        {
            var query = $"SELECT * FROM  MIRSANIDEV.HR_LOANS_TYPE WHERE HRLT_SYS_ID = {hrLoansType} OR {hrLoansType} = 0";

            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostHrLoansTypeData(List<HrLoansType> hrLoansType, string authParms)
        {
            foreach (var item in hrLoansType)
            {

                if (item.HRLT_SYS_ID > 0) item.STATE = (int)OperationType.Update;
                else item.STATE = (int)OperationType.Add;
                item.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_HR_LOANS_TYPE_XML", hrLoansType.ToList<dynamic>(), authParms);

        }










    }
}
