using System.Data;
using Mersani.Oracle;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mersani.models.FinancialSetup;
using Oracle.ManagedDataAccess.Client;
using Mersani.Interfaces.FinancialSetup;

namespace Mersani.Repositories.FinancialSetup
{
    public class OwnerSupplierRepository : OwnerSupplierRepo
    {
        public async Task<DataSet> BulkInsertUpdateOwnerSupplierData(OwnerSupplier entity, string authParms)
        {
            if (entity.FOS_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
            else entity.STATE = (int)OperationType.Add;
            entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            entity.FOS_V_CODE = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;

            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_OWNERSUPPLIER_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> DeleteOwnerSupplierData(OwnerSupplier entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_OWNERSUPPLIER_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetOwnerSupplierDataList(OwnerSupplier entity, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $"SELECT FOS.*," +
                $"       'OW' || FOS.FOS_OWNER_COMP_SYS_ID        AS OWNER_V_CODE," +
                $"       GET_ACC_NO_FROM_CODE (FOS_ACC_CODE) AS ACC_NO" +
                $"  FROM FINS_OWNER_suppLIER FOS" +
                $" WHERE  FOS.FOS_FRZ_Y_N = 'N'" +
                $"       AND(('OW' || FOS.FOS_OWNER_COMP_SYS_ID = FUN_GET_PARENT_V_CODE('" +authP.User_Act_PH+"'))" +
                $"            OR 'CM' || FOS.FOS_OWNER_COMP_SYS_ID = FUN_GET_PARENT_V_CODE('" + authP.User_Act_PH + "'))";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pFOS_SYS_ID", entity.FOS_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetLastCode(string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $"SELECT  NVL (MAX (TO_NUMBER (CASE WHEN REGEXP_LIKE (FOS_CODE, '^[0-9]+') THEN FOS_CODE ELSE '0' END)), 0) + 1 AS Code  FROM FINS_OWNER_SUPPLIER FOS " +
                $" where (('OW' || FOS.FOS_OWNER_COMP_SYS_ID = FUN_GET_PARENT_V_CODE('"+authP.User_Act_PH+"'))"+
                $" OR 'CM' || FOS.FOS_OWNER_COMP_SYS_ID = FUN_GET_PARENT_V_CODE('"+ authP.User_Act_PH + "'))";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

    }
}
