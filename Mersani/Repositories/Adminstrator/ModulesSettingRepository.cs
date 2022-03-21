using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class ModulesSettingRepository : IModulesSettingRepo
    {
        public async Task<DataSet> BulkModulesSetting(List<ModulesSetting> entities, string authParms)
        {
            foreach (ModulesSetting entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.GMS_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_MODULES_SETTING_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteModulesSetting(ModulesSetting entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_MODULES_SETTING_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetModulesSetting(ModulesSetting entity, string authParms)
        {
            var query = $"SELECT * FROM GAS_MODULES_SETTING WHERE (GMS_SYS_ID = :pGMS_SYS_ID OR :pGMS_SYS_ID = 0)";
            var parms = new List<OracleParameter>() { new OracleParameter("pGMS_SYS_ID", entity.GMS_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetSettingByType(string type, string authParms)
        {
            var query = $"SELECT * FROM GAS_MODULES_SETTING WHERE GMS_TYPE = :pGMS_TYPE";
            var parms = new List<OracleParameter>() { new OracleParameter("pGMS_TYPE", type) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
    }
}
