using System.Data;
using Mersani.Oracle;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mersani.models.FinancialSetup;
using Oracle.ManagedDataAccess.Client;
using Mersani.Interfaces.FinancialSetup;

namespace Mersani.Repositories.FinancialSetup
{
    public class SupplierRepository : ISupplierRepo
    {
        public async Task<DataSet> BulkInsertUpdateSupplierData(Supplier entity, string authParms)
        {
            if (entity.SUPP_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
            else entity.STATE = (int)OperationType.Add;
            entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            entity.SUPP_V_CODE = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;

            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_SUPPLIER_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> DeleteSupplierData(Supplier entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_SUPPLIER_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetSupplierDataList(Supplier entity, string authParms)
        {
            var query = $"select supp.* from FINS_SUPPLIER supp " +// left JOIN fins_account acnt ON supp.SUPP_ACC_CODE = acnt.ACC_CODE " +
                $"WHERE (SUPP_SYS_ID = :pSUPP_SYS_ID OR :pSUPP_SYS_ID = 0) ";//AND SUPP_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pSUPP_SYS_ID", entity.SUPP_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (SUPP_CODE, '^[0-9]+') THEN SUPP_CODE ELSE '0' END)), 0) + 1 AS Code FROM FINS_SUPPLIER ";
              //  $"WHERE SUPP_V_CODE = FUN_GET_PARENT_V_CODE('{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}')";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
    }
}
