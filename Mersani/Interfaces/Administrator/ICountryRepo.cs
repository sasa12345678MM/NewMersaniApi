using Mersani.models.Administrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
    public interface ICountryRepo
    {
        Task<DataSet> GetCountries(int id, string authParms);
        Task<DataSet> GetCountryByName(string Name, string authParms);

        Task<DataSet> PostCountry(Country country, string authParms);
        Task<DataSet> DeleteCountry(Country entity, string authParms);

        Task<DataSet> GetLastCode(string authParms);
    }
}
