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
    public class HrIncrementsTypesRepository : HrIncrementsTypesRepo
    {
        public async Task<DataSet> GetHrIncrementsTypesData(int hrIncrementsTypes, string authParms)
        {
          var query = $"SELECT * FROM  MIRSANIDEV.HR_INCREMENTS_TYPES WHERE HRIT_SYS_ID = {hrIncrementsTypes} OR {hrIncrementsTypes} = 0";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
        public async Task<DataSet> PostHrIncrementsTypesData(List<HrIncrementsTypes> entities, string authParms)
        {
            foreach (var HrIncrementsTypes in entities)
            {

               if (HrIncrementsTypes.HRIT_SYS_ID > 0) HrIncrementsTypes.STATE = (int)OperationType.Update;
               else HrIncrementsTypes.STATE = (int)OperationType.Add;
               HrIncrementsTypes.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_INCREMENTS_TYPES_XML", entities.ToList<dynamic>(), authParms);
          
        }
        public async Task<DataSet> DeleteHrIncrementsTypesData(HrIncrementsTypes hrIncrementsTypes, string authParms)
        {
            hrIncrementsTypes.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_INCREMENTS_TYPES_XML", new List<dynamic>() { hrIncrementsTypes }, authParms);
        }


    }
}
