using Mersani.Interfaces.Stock;
using Mersani.models.Stock;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Repositories.Stock
{
    public class InventoryLocationsRepository : IInventoryLocationsRepo
    {
        public async Task<DataSet> GetInventoryLocations(InventoryLocations entity, string authParms)
        {
            var query = $"SELECT * FROM INV_INVENTORY_LOCATIONS WHERE IIL_MST_INV_SYS_ID = :pSYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pSYS_ID", entity.IIL_MST_INV_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetInventoryLocationsById(InventoryLocations entity, string authParms)
        {
            var query = $"SELECT * FROM INV_INVENTORY_LOCATIONS WHERE IIL_LOC_SYS_ID = :pSYS_ID OR :pSYS_ID = 0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pSYS_ID", entity.IIL_LOC_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostInventoryLocations(InventoryLocations entity, string authParms)
        {
            entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;

            if (entity.IIL_LOC_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
            else entity.STATE = (int)OperationType.Add;

            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_INVNTRY_LOCATIONS_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> DeleteInventoryLocations(InventoryLocations entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_INVNTRY_LOCATIONS_XML", new List<dynamic>() { entity }, authParms);
        }
        public async Task<DataSet> GetLastCode(int id, string authParms)
        {
            var query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (IIL_LOC_CODE, '^[0-9]+') THEN IIL_LOC_CODE ELSE '0' END)), 0) + 1 AS Code FROM INV_INVENTORY_LOCATIONS WHERE IIL_MST_INV_SYS_ID = {id}";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

    }
}
