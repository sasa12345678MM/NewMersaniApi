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
    public class HrDeductionRepository : DeductionTypeRepo
    {
        public async Task<DataSet> DeleteHrDeductionTypeData(DeductionType deductionType, string authParms)
        {
            deductionType.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_DeductionType_XML", new List<dynamic>() { deductionType }, authParms);
        }

        public async Task<DataSet> GetHrDeductionTypeData(int deductionType, string authParms)
        {
            var query = $"SELECT * FROM  MIRSANIDEV.HR_DEDUCTION WHERE HRD_SYS_ID = {deductionType} OR {deductionType} = 0";

            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostHrDeductionTypeData(List<DeductionType> deductionType, string authParms)
        {
            foreach (var item in deductionType)
            {

                if (item.HRD_SYS_ID > 0) item.STATE = (int)OperationType.Update;
                else item.STATE = (int)OperationType.Add;
                item.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_DeductionType_XML", deductionType.ToList<dynamic>(), authParms);
        }

        
    }
}
