using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class CompanyBranchesRepository : ICompanyBranchesRepo
    {
        public async Task<DataSet> GetBranchByCompany(CompanyBranches entity, string authParms)
        {
            var query = "SELECT CB.CB_ID, CB.CB_SYS_ID, CB.CB_TXN_CODE, CB.CB_NAME_AR, CB.CB_NAME_EN, CB.CB_COMPANY_SYS_ID, CB.CB_CITY_SYS_ID, CB.CB_REMINDER_Y_N, " +
                        "CTY.CITY_NAME_AR, CTY.CITY_NAME_EN, CMP.COMP_NAME_AR, CMP.COMP_NAME_EN FROM GAS_COMPANY_BRANCHES CB " +
                        "LEFT OUTER JOIN GAS_CITY CTY ON CTY.CITY_SYS_ID = CB.CB_CITY_SYS_ID " +
                        "LEFT OUTER JOIN GAS_COMPANY CMP ON CMP.COMP_SYS_ID = CB.CB_COMPANY_SYS_ID " +
                        "WHERE CB.CB_COMPANY_SYS_ID = :pCB_COMPANY_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pCB_COMPANY_SYS_ID", entity.CB_COMPANY_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetBranch(CompanyBranches entity, string authParms)
        {
            var query = "SELECT CB.CB_ID, CB.CB_SYS_ID, CB.CB_TXN_CODE, CB.CB_NAME_AR, CB.CB_NAME_EN, CB.CB_COMPANY_SYS_ID, CB.CB_CITY_SYS_ID, CB.CB_REMINDER_Y_N, " +
                        "CTY.CITY_NAME_AR, CTY.CITY_NAME_EN, CMP.COMP_NAME_AR, CMP.COMP_NAME_EN FROM GAS_COMPANY_BRANCHES CB " +
                        "LEFT OUTER JOIN GAS_CITY CTY ON CTY.CITY_SYS_ID = CB.CB_CITY_SYS_ID " +
                        "LEFT OUTER JOIN GAS_COMPANY CMP ON CMP.COMP_SYS_ID = CB.CB_COMPANY_SYS_ID " +
                        "WHERE CB.CB_SYS_ID = :pCB_SYS_ID OR :pCB_SYS_ID = 0";
            var parms = new List<OracleParameter>() { new OracleParameter("pCB_SYS_ID", entity.CB_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkBranches(List<CompanyBranches> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                if (entity.CB_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_COMPANY_BRANCHES_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteBranch(CompanyBranches entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_COMPANY_BRANCHES_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetLastCode(int id, string authParms)
        {
            var query = $"SELECT  NVL (MAX (TO_NUMBER (CB_ID)), 0) + 1 AS Code FROM GAS_COMPANY_BRANCHES WHERE CB_COMPANY_SYS_ID = {id}";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
    }
}
