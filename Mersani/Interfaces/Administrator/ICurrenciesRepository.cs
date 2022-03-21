using Mersani.models.Administrator;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
    public interface ICurrenciesRepository
    {
        //List<Currencies> GetCurrencies(int id, string authParms);
        //List<Currencies> SearchCurrencies(Currencies criteria, string authParms);
        //bool PostNewCurrency(Currencies currency, string authParms);
        //bool UpdateCurrency(int id, Currencies currency, string authParms);
        //bool DeleteCurrency(int id, string authparms);

        Task<DataSet> GetCurrencyDataList(Currencies entity, string authParms);
        Task<DataSet> BulkInsertUpdateCurrency(List<Currencies> entities, string authParms);
        Task<DataSet> DeleteCurrencyData(Currencies entity, string authParms);

        // rates
        Task<DataSet> BulkCurrencyRates(CurrencyRate entity, string authParms);
        Task<DataSet> GetCurrencyRates(CurrencyRate entity, string authParms);
        Task<DataSet> DeleteCurrencyRates(CurrencyRate entity, string authParms);

        Task<DataSet> GetLastCode(string authParms);
    }
}
