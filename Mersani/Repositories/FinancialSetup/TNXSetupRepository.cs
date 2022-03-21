using System.Data;
using Mersani.Oracle;
using System.Threading.Tasks;
using Mersani.models.FinancialSetup;
using System.Collections.Generic;
using Mersani.Interfaces.FinancialSetup;
using Oracle.ManagedDataAccess.Client;
using System.Linq;

namespace Mersani.Repositories.FinancialSetup
{
    public class TNXSetupRepository: ITNXSetupRepo
    {
        public async Task<DataSet> GetTXNSetupDataList(TXNSetup entity, string authParms)
        {
            var query = $"SELECT * FROM FINS_TXN_SETUP WHERE FTS_SYS_ID = :pFTS_CODE OR :pFTS_CODE = 0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pFTS_CODE", entity.FTS_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkInsertUpdateTXNSetup(List<TXNSetup> entities, string authParms)
        {
            foreach (TXNSetup entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if(entity.FTS_SYS_ID > 0)
                {
                    entity.STATE = (int)OperationType.Update;
                }
                else
                {
                    entity.STATE = (int)OperationType.Add;
                }
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_TXN_SETUP_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteTXNSetupData(TXNSetup entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_TXN_SETUP_XML", new List<dynamic>() { entity }, authParms);
        }
    }
}
