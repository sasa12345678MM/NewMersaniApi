using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Oracle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class CountryLoyalityRepository : CountryLoyalityRepo
    {
   

        public async Task<DataSet> geCountryLoyality(CountryLoyalitySetup CountryLoyalitySetup, int parentId, string authParms)
        {
            var query = $" SELECT * from GAS_COUNTRY_LOYALITY_SETUP " ;

            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostCountryLoyality(CountryLoyalitySetup entity, string authParms)
        {
            var Auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            if (entity.GCLS_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
            else entity.STATE = (int)OperationType.Add;
            entity.CURR_USER = Auth.UserCode;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_COUNTRY_LOYALITY_SETUP_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> DeleteCountryLoyality(CountryLoyalitySetup CountryLoyalitySetup, string authParms)
        {
            var Auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            CountryLoyalitySetup.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_COUNTRY_LOYALITY_SETUP_XML", new List<dynamic>() { CountryLoyalitySetup }, authParms);
        }
    }
}
