using Mersani.Interfaces.Stock;
using Mersani.models.Stock;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Stock
{
    public class ItemGroupsRepository : IItemGroupsRepo
    {
        public async Task<DataSet> GetItemsGroups(ItemGroups entity, string authParms)
        {
            var query = $"SELECT * FROM INV_ITEM_GROUP WHERE IIG_SYS_ID = :pIIG_SYS_ID OR :pIIG_SYS_ID = 0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pIIG_SYS_ID", entity.IIG_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        
        public async Task<DataSet> BulkItemsGroups(List<ItemGroups> entities, string authParms)
        {
            foreach (ItemGroups entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.IIG_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_GROUP_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteItemGroup(ItemGroups entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_GROUP_XML", new List<dynamic>() { entity }, authParms);
        }
    }
}
