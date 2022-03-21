using Mersani.Interfaces.Finance;
using Mersani.models.Finance;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Finance
{
    public class PeriodYearRepository : IPeriodYearRepo
    {

        public async Task<DataSet> GetFinancialYear(FinsYear entity, string authParms)
        {
            var query = $"SELECT * FROM FINS_YEARS WHERE YEAR_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetFinancialPeriods(FinsYear entity, string authParms)
        {
            var query = $"SELECT * FROM FINS_PERIOD WHERE PERIOD_YEAR = :pYear AND PERIOD_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() { new OracleParameter("pYear", entity.PERIOD_YEAR) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostFinancialYearPeriods(FinancialYearPeriods entity, string authParms)
        {
            var authData = OracleDQ.GetAuthenticatedUserObject(authParms);

            //hdr
            entity.YEAR.YEAR_V_CODE = authData.User_Act_PH;
            entity.YEAR.CURR_USER = authData.UserCode.Value;
            if (entity.YEAR.YEAR_SYS_ID > 0) entity.YEAR.STATE = (int)OperationType.Update;
            else entity.YEAR.STATE = (int)OperationType.Add;

            // dtl 
            for (int i = 0; i < entity.PERIODS.Count; i++)
            {
                entity.PERIODS[i].PERIOD_YEAR = entity.YEAR.PERIOD_YEAR;
                entity.PERIODS[i].PERIOD_V_CODE = authData.User_Act_PH;
                entity.PERIODS[i].CURR_USER = authData.UserCode;
                if (entity.PERIODS[i].PERIOD_SYS_ID > 0) entity.PERIODS[i].STATE = (int)OperationType.Update;
                else entity.PERIODS[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entity.YEAR });
            parameters.Add("xml_document_d", entity.PERIODS.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_FINS_YEARS_PERIOD_XML", parameters, authParms);
        }

        public async Task<DataSet> DeleteFinancialYear(FinsYear entity, int type, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteDeleteProcAsync("PRC_FINS_YEARS_PERIOD_DEL", new { code = type == 1 ? entity.YEAR_SYS_ID : entity.PERIOD_YEAR, type = type }, authParms);
        }
    }


}
