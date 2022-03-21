using Mersani.Interfaces.HR;
using Mersani.models.HR;
using Mersani.Oracle;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.HR
{
    public class HrVacationRepository : HrVacationRepo
    {
        public async Task<DataSet> GetHrVacationsData(int hrVacations, string authParms)
        {
            var query = $"SELECT * FROM  MIRSANIDEV.HR_VACATIONS_TYPE WHERE HRVT_SYS_ID = {hrVacations} OR {hrVacations} = 0";

            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostHrVacationsData(List<HrVacations> entities, string authParms)
        {
            foreach (var HrVacation in entities)
            {

                if (HrVacation.HRVT_SYS_ID > 0) HrVacation.STATE = (int)OperationType.Update;
                else HrVacation.STATE = (int)OperationType.Add;
                HrVacation.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_VACATIONS_TYPE_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteHrVacationsData(HrVacations hrVacations, string authParms)
        {
            hrVacations.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_VACATIONS_TYPE_XML", new List<dynamic>() { hrVacations }, authParms);
        }
    }
}
