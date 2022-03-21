using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class CityRepository : ICityRepo
    {
        public async Task<DataSet> GetCity(int id, string authParms)
        {
            var query = $"SELECT CTY.* FROM GAS_CITY CTY WHERE CTY.CITY_REGION_SYS_ID = :pCITY_REGION_SYS_ID OR :pCITY_REGION_SYS_ID = 0";
            var parms = new List<OracleParameter>() { new OracleParameter("pCITY_REGION_SYS_ID", id) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text,_public:true);
        }

        public async Task<DataSet> PostCity(City City, string authParms)
        {
                if (City.CITY_SYS_ID > 0) City.STATE = (int)OperationType.Update;
                else City.STATE = (int)OperationType.Add;
                City.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_CITY_XML", new List<dynamic>() { City }, authParms);
        }

        public async Task<DataSet> DeleteCity(City entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_CITY_XML", new List<dynamic>() { entity }, authParms);
        }
        public async Task<DataSet> GetLastCode(int id, string authParms)
        {
            var query = $"SELECT  NVL (MAX (TO_NUMBER (CITY_ID)), 0) + 1 AS Code FROM GAS_CITY WHERE CITY_REGION_SYS_ID = {id}";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
    }

}
