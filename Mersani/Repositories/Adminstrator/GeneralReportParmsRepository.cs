using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class GeneralReportParmsRepository : IGeneralReportParmsRepo
    {
        public async Task<DataSet> bulkGeneralReportParms(List<GeneralReportParms> GeneralReportParms,   string authParms)
        {
            
                foreach (GeneralReportParms entity in GeneralReportParms)
                {
                    entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                    if (entity.GRP_SYS_ID > 0)
                    {
                        entity.STATE = (int)OperationType.Update;
                    }
                    else
                    {
                        entity.STATE = (int)OperationType.Add;
                    }
                }
                return await OracleDQ.ExcuteXmlProcAsync("PRC_GENERAL_REPORT_PARMS_XML", GeneralReportParms.ToList<dynamic>(), authParms);
            

        }

        public async Task<DataSet> deleteGeneralReportParms(GeneralReportParms GeneralReportParms, string authParms)
        {
            GeneralReportParms.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GENERAL_REPORT_PARMS_XML", new List<dynamic>() { GeneralReportParms }, authParms);
        }

        public async Task<DataSet> GetGeneralReportParms(GeneralReportParms reportId, string authParms)
        {
            var query = $"SELECT * FROM GAS_GNRL_REPORTS_PARM WHERE GRP_SYS_ID = :pGRP_SYS_ID OR :pGRP_SYS_ID = 0 ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pGRP_SYS_ID", reportId.GRP_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

       
    }
}
