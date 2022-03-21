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
    public class HrBanksRepository : HrBanksRepo
    {
        public async Task<DataSet> GetHrBanksData(int HrBanks, string authParms)
        {
            var query = $"SELECT * FROM  MIRSANIDEV.HR_EMP_BANKS WHERE HREB_SYS_ID = {HrBanks} OR {HrBanks} = 0";

            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
        public async Task<DataSet> PostHrBanksData(List<HrBanks> entities, string authParms)
        {
            foreach (var HrBanks in entities)
            {

                if (HrBanks.HREB_SYS_ID > 0) HrBanks.STATE = (int)OperationType.Update;
                else HrBanks.STATE = (int)OperationType.Add;
                HrBanks.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_Banks_XML", entities.ToList<dynamic>(), authParms);
        }


        public async Task<DataSet> DeleteHrBanksData(HrBanks HrBanks, string authParms)
        {
            HrBanks.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_Banks_XML", new List<dynamic>() { HrBanks }, authParms);
        }

      
    }
}
