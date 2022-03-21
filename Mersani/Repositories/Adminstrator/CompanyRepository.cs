using System.Collections.Generic;
using Mersani.models.Administrator;
using Mersani.Interfaces.Administrator;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

namespace Mersani.Repositories.Adminstrator
{
    public class CompanyRepository : ICompanyRepo
    {
        public async Task<DataSet> GetCompaniesByGroup(Company entity, string authParms)
        {
            var query = $"SELECT COMP.*, GRP.GROUP_NAME_EN, GRP.GROUP_NAME_AR, CTR.C_NAME_EN, CTR.C_NAME_AR FROM GAS_COMPANY COMP " +
                        $"LEFT JOIN GAS_GROUP GRP ON COMP.COMP_GROUP_SYS_ID = GRP.GROUP_SYS_ID " +
                        $"LEFT JOIN GAS_COUNTRY CTR ON COMP.COMP_COUNTRY_SYS_ID = CTR.C_SYS_ID " +
                        $"WHERE COMP.COMP_GROUP_SYS_ID = :pCOMP_GROUP_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pCOMP_GROUP_SYS_ID", entity.COMP_GROUP_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetCompany(Company entity, string authParms)
        {
            var query = $"SELECT COMP.*, GRP.GROUP_NAME_EN, GRP.GROUP_NAME_AR, CTR.C_NAME_EN, CTR.C_NAME_AR FROM GAS_COMPANY COMP " +
                        $"LEFT JOIN GAS_GROUP GRP ON COMP.COMP_GROUP_SYS_ID = GRP.GROUP_SYS_ID " +
                        $"LEFT JOIN GAS_COUNTRY CTR ON COMP.COMP_COUNTRY_SYS_ID = CTR.C_SYS_ID " +
                        $"WHERE COMP.COMP_SYS_ID = :pCOMP_SYS_ID OR :pCOMP_SYS_ID = 0";
            var parms = new List<OracleParameter>() { new OracleParameter("pGROUP_SYS_ID", entity.COMP_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> BulkCompanys(List<Company> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                if (entity.COMP_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_COMPANY_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeleteCompany(Company entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_COMPANY_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetLastCode(int id, string authParms)
        {
            var query = $"SELECT  NVL (MAX (TO_NUMBER (COMP_ID)), 0) + 1 AS Code FROM GAS_COMPANY WHERE COMP_GROUP_SYS_ID = {id}";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

    }
}
