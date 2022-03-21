using System;
using Mersani.Oracle;
using System.Collections.Generic;
using Mersani.models.Administrator;
using Mersani.Interfaces.Administrator;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

namespace Mersani.Controllers.Administrator
{
    public class CurrenciesRepository : ICurrenciesRepository
    {
        public async Task<DataSet> BulkCurrencyRates(CurrencyRate entity, string authParms)
        {
            if (entity.CURRR_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
            else entity.STATE = (int)OperationType.Add;
            entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;

            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_CURRENCY_EX_RATE_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetCurrencyRates(CurrencyRate entity, string authParms)
        {
            var query = $"select rate.*, curr.CURR_NAME_AR, curr.CURR_NAME_EN from GAS_CURRENCY_EX_RATE rate " +
                $"join GAS_CURRENCY curr on curr.CURR_SYS_ID = rate.CURRR_DET_CURR_SYS_ID " +
                $"WHERE CURRR_MAIN_CURR_SYS_ID = :pCURRR_CURR_SYS_ID OR :pCURRR_CURR_SYS_ID = 0";
            var parms = new List<OracleParameter>() { new OracleParameter("pCURRR_CURR_SYS_ID", entity.CURRR_MAIN_CURR_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> DeleteCurrencyRates(CurrencyRate entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_CURRENCY_EX_RATE_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetCurrencyDataList(Currencies entity, string authParms)
        {
            var query = "SELECT CURR.* FROM GAS_CURRENCY CURR WHERE CURR.CURR_SYS_ID = :pCURR_SYS_ID OR :pCURR_SYS_ID = 0";
            var parms = new List<OracleParameter>() { new OracleParameter("pCURR_SYS_ID", entity.CURR_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text,_public:true);
        }

        public async Task<DataSet> BulkInsertUpdateCurrency(List<Currencies> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                if (entity.CURR_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_CURRENCY_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteCurrencyData(Currencies entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_CURRENCY_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX (TO_NUMBER (CURR_ID)), 0) + 1 AS Code FROM GAS_CURRENCY";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
    }
}
