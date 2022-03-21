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
    public class HrBonusesRepository : HrBonusesRepo
    {
        public async Task<DataSet> GetHrBonusesData(int hrBounses, string authParms)
        {
            var query = $"SELECT * FROM  MIRSANIDEV.HR_BONUSES WHERE HRB_SYS_ID = {hrBounses} OR {hrBounses} = 0";

            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostHrBonusesData(List<HrBonuses> entities, string authParms)
        {
            foreach (var HrBonuses in entities)
            {

                if (HrBonuses.HRB_SYS_ID > 0) HrBonuses.STATE = (int)OperationType.Update;
                else HrBonuses.STATE = (int)OperationType.Add;
                HrBonuses.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_Bonuses_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeleteHrBonusesData(HrBonuses hrBounses, string authParms)
        {
            hrBounses.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_Bonuses_XML", new List<dynamic>() { hrBounses }, authParms);
        }

    }
}

