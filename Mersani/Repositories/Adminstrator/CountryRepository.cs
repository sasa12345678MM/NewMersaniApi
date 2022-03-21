using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class CountryRepository : ICountryRepo
    {   
        public async Task<DataSet> DeleteCountry(Country entity, string  authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_COUNTRY_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> PostCountry(Country country, string authParms)
        {
            if (country.C_SYS_ID > 0) country.STATE = (int)OperationType.Update;
            else country.STATE = (int)OperationType.Add;
            country.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_COUNTRY_XML", new List<dynamic>() { country }, authParms); ;
        }

        public async Task<DataSet> GetLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX (TO_NUMBER (C_COUNTRY_ID)), 0) + 1 AS Code FROM GAS_COUNTRY";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetCountries(int id, string authParms)
        {
            var query = $"select * from gas_country where c_sys_id = :pSYS_ID OR :pSYS_ID = 0";
            var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", id) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text,_public:true);
        }

        public async Task<DataSet> GetCountryByName(string Name, string authParms)
        {
            var query = $"select * from gas_country where C_NAME_EN ='{Name}'";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text, _public: true);
        }
    }

}





