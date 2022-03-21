using Mersani.models.Administrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
    public interface CountryLoyalityRepo
    {
        Task<DataSet> geCountryLoyality(CountryLoyalitySetup CountryLoyalitySetup, int parentId, string authParms);
        Task<DataSet> PostCountryLoyality(CountryLoyalitySetup CountryLoyalitySetup, string authParms);
        Task<DataSet> DeleteCountryLoyality(CountryLoyalitySetup CountryLoyalitySetup, string authParms);
    }
}
