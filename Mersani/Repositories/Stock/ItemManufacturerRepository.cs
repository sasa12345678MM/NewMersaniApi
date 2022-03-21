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
    public class ItemManufacturerRepository : IItemManufacturerRepo
    {
        public async Task<DataSet> GetItemManufacturers(StockItemManufacturer entity, string authParms)
        {
            var query = $"SELECT manf.*, cnty.C_NAME_AR, cnty.C_NAME_EN " +
                $" FROM INV_ITEM_MANUFACTURER manf " +
                $" join gas_country cnty on cnty.C_SYS_ID = manf.IIMF_CNTRY_SYS_ID " +
                $" WHERE manf.IIMF_SYS_ID = :pIIMF_SYS_ID or :pIIMF_SYS_ID = 0 ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pIIMF_SYS_ID", entity.IIMF_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text, _public: true);
        }

        public async Task<DataSet> BulkItemManufacturers(List<StockItemManufacturer> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.IIMF_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_MANUFACTURER_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteItemManufacturer(StockItemManufacturer entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_MANUFACTURER_XML", new List<dynamic>() { entity }, authParms);
        }
    }
}
