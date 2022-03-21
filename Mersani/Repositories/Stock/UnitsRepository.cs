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
    public class UnitsRepository : IUnitsRepo
    {
        public async Task<DataSet> GetUnits(Units entity, string authParms)
        {
            var query = $"SELECT * FROM INV_UOM WHERE UOM_SYS_ID = :pUOM_SYS_ID or :pUOM_SYS_ID=0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pUOM_SYS_ID", entity.UOM_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkUnits(List<Units> entities, string authParms)
        {
            foreach (Units entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.UOM_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_UOM_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteUnit(Units entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_UOM_XML", new List<dynamic>() { entity }, authParms);
        }
    }
}
