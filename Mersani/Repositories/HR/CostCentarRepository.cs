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
    public class CostCentarRepository : CostCenterRepo
    {
        public async Task<DataSet> GetHrCostCenterData(int hr, string authParms)
        {
            var query = $"SELECT * FROM  MIRSANIDEV.HR_COST_CENTERS WHERE HRCC_SYS_ID = {hr} OR {hr} = 0";


            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
        public async Task<DataSet> DeleteHrCostCenterData(CostCentar costCentar, string authParms)
        {
            costCentar.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_COSTCENTER_XML", new List<dynamic>() { costCentar }, authParms);
        }
        public async Task<DataSet> PostHrCostCenterData(List<CostCentar> costCentar, string authParms)
        {
            foreach (var CostCentar in costCentar)
            {

                if (CostCentar.HRCC_SYS_ID > 0) CostCentar.STATE = (int)OperationType.Update;
                else CostCentar.STATE = (int)OperationType.Add;
                CostCentar.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }
            return await OracleDQ.ExcuteXmlProcAsync("MIRSANIDEV.PRC_HR_COSTCENTER_XML", costCentar.ToList<dynamic>(), authParms); 
        }
    }


}
