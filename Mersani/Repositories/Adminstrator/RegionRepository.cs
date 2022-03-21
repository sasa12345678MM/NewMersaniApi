using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class RegionRepository : IRegionRepo
    {

        public async Task<DataSet> GetRegions(int id, string authParms)
        {
            var query = $"SELECT REGN.* FROM GAS_REGION REGN WHERE REGN.R_COUNTRY_SYS_ID = :pR_COUNTRY_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pR_COUNTRY_SYS_ID", id) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text,_public:true);
        }

        public async Task<DataSet> PostRegion(Region region, string authParms)
        {
            if (region.R_SYS_ID > 0) region.STATE = (int)OperationType.Update;
            else region.STATE = (int)OperationType.Add;
            region.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_REGION_XML", new List<dynamic>() { region }, authParms);
        }

        public async Task<DataSet> DeleteRegion(Region entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_REGION_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetLastCode(int id, string authParms)
        {
            var query = $"SELECT  NVL (MAX (TO_NUMBER (R_REGION_ID)), 0) + 1 AS Code FROM GAS_REGION WHERE R_COUNTRY_SYS_ID = {id}";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
    }
}



