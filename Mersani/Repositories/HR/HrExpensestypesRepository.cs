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
    public class HrExpensestypesRepository : HrExpensestypesRepo
    {
        public async Task<DataSet> PostHrExpensestypesData(List<HrExpensestypes> hrExpensestypes, string authParms)
        {
            foreach (var HrExpensestypes in hrExpensestypes)
            {

                if (HrExpensestypes.HRET_SYS_ID > 0) HrExpensestypes.STATE = (int)OperationType.Update;
                else HrExpensestypes.STATE = (int)OperationType.Add;
                HrExpensestypes.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_EXPENSES_TYPES_XML", hrExpensestypes.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> GetHrExpensestypesData(int hrExpensestypes, string authParms)
        {
            var query = $"SELECT * FROM  MIRSANIDEV.HR_EXPENSES_TYPES WHERE HRET_SYS_ID = {hrExpensestypes} OR {hrExpensestypes} = 0";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
        public async Task<DataSet> DeleteHrExpensestypesData(HrExpensestypes hrExpensestypes, string authParms)
        {
            hrExpensestypes.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_EXPENSES_TYPES_XML", new List<dynamic>() { hrExpensestypes }, authParms);
        }


    }
}
