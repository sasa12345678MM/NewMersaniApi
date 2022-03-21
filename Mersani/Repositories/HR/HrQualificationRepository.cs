using Mersani.Interfaces.HR;
using Mersani.models.HR;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.HR
{
    public class HrQualificationRepository : HrQualificationRepo
    {       
        public async Task<DataSet> GetHrQualificationData(int HrQualification, string authParms)
        {
            var query = $"SELECT * FROM  MIRSANIDEV.HR_QUALIFICATIONS WHERE HRQ_SYS_ID = {HrQualification} OR {HrQualification} = 0";

            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
       
        public async Task<DataSet> DeleteHrQualificationData(HrQualification HrQualification, string authParms)
        {
            HrQualification.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_QUALIFICATIONS_XML", new List<dynamic>() { HrQualification }, authParms);
        }

       
        public async Task<DataSet> PostHrQualificationListData(List<HrQualification> entities, string authParms)
        {
            foreach (var HrQualification in entities)
            {

                if (HrQualification.HRQ_SYS_ID > 0) HrQualification.STATE = (int)OperationType.Update;
                else HrQualification.STATE = (int)OperationType.Add;
                HrQualification.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_QUALIFICATIONS_XML", entities.ToList<dynamic>(), authParms);
        }

        
    }
}
