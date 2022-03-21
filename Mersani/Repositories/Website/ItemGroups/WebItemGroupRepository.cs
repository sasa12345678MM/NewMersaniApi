using Mersani.Interfaces.Website.ItemGroups;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mersani.models.Stock;
using Oracle.ManagedDataAccess.Client;
using Mersani.Oracle;

namespace Mersani.Repositories.Website.ItemGroups
{
    public class WebtemGroupRepository : IWebItemGroup
    {
        public async Task<DataSet> GetItemsGroups(Mersani.models.Stock.ItemGroups entity, string authParms)
        {
            var query = $"SELECT * FROM INV_ITEM_GROUP WHERE IIG_SYS_ID = :pIIG_SYS_ID OR :pIIG_SYS_ID = 0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pIIG_SYS_ID", entity.IIG_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text, _public: true);
        }
        public async Task<DataSet> GetItemsGroupsByLevel(string levels ,string authParms)
        {
            var query = $"SELECT * FROM INV_ITEM_GROUP WHERE IIG_LEVEL in ({levels}) AND IIG_STK_SRV_S_V = 'S' AND IIG_FRZ_Y_N = 'N'";
            //var parms = new List<OracleParameter>() { new OracleParameter("pIIG_LEVEL", levels) };
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text, _public: true);
        }
        public async Task<DataSet> GetItemsGroupChildren(int GroupId, string authParms)
        {
            var query = $"SELECT * FROM INV_ITEM_GROUP WHERE IIG_PARENT_SYS_ID =:PARENT_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PARENT_ID", GroupId)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text, _public: true);
        }


     
    }
}
