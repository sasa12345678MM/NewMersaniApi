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
    public class ItemDosageRepository : IItemDosageRepo
    {
        public async Task<DataSet> GetItemDosages(StockItemDosage entity, string authParms)
        {
            var query = $"SELECT * FROM INV_ITEM_DOSAGE_FORM WHERE IIDF_SYS_ID = :pIIDF_SYS_ID or :pIIDF_SYS_ID=0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pIIDF_SYS_ID", entity.IIDF_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkItemDosages(List<StockItemDosage> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.IIDF_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_DOSAGE_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteItemDosage(StockItemDosage entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_DOSAGE_XML", new List<dynamic>() { entity }, authParms);
        }
    }
}
