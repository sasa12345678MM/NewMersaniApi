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
    public class GeneralsetupRepository : IGeneralSetupRepo
    {
        // master methods
        public async Task<DataSet> GetMasterData(GeneralSetupMaster master, string authParms)
        {
            var query = $"SELECT * FROM GAS_GNRL_SET_HDR WHERE GSH_SYS_ID = :pCode OR :pCode = 0";
            var parms = new List<OracleParameter>() { new OracleParameter("pCode", master.GSH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostMasterData(GeneralSetupMaster master, string authParms)
        {
            if (master.GSH_SYS_ID > 0) master.STATE = (int)OperationType.Update;
            else master.STATE = (int)OperationType.Add;
            master.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_GNRL_SET_HDR_XML", new List<dynamic>() { master }, authParms);
        }

        public async Task<DataSet> DeleteMasterData(GeneralSetupMaster master, string authParms)
        {
            master.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_GNRL_SET_HDR_XML", new List<dynamic>() { master }, authParms);
        }

        // detail methods
        public async Task<DataSet> GetDetailsData(GeneralSetupDetail detail, string authParms)
        {
            var query = $"SELECT * FROM GAS_GNRL_SET_DTL WHERE GSD_GSH_SYS_ID = :pCode";
            var parms = new List<OracleParameter>() { new OracleParameter("pCode", detail.GSD_GSH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkDetailsData(List<GeneralSetupDetail> details, string authParms)
        {
            foreach (var entity in details)
            {
                if (entity.GSD_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_GNRL_SET_DTL_XML", details.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteDetailsData(GeneralSetupDetail detail, string authParms)
        {
            detail.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_GNRL_SET_DTL_XML", new List<dynamic>() { detail }, authParms);
        } 
    }
}
