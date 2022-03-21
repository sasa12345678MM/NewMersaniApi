using Mersani.Interfaces.FinancialSetup;
using Mersani.models.FinancialSetup;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.FinancialSetup
{
    public class BudgetTypeRepository: IBudgetTypeRepo
    {
        public async Task<bool> DeleteBudgetType(int id, string authParms)
        {
            var dyParam = GetDynamicParameters(new BudgetType() { BDG_CODE = id }, authParms, OperationType.Delete);
            return await OracleDQ.PostDataAsync("PRC_FINS_BUDGET_TYPES_DEL", authParms, dyParam, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<BudgetType>> GetBudgetType(int id, string authParms)
        {
            var query = $"SELECT * FROM FINS_BUDGET_TYPES WHERE BDG_CODE = :pBDG_CODE OR :pBDG_CODE = 0";
            return await OracleDQ.GetDataAsync<BudgetType>(query, authParms, new { pBDG_CODE = id });
        }
        public async Task<bool> PostNewBudgetType(BudgetType entity, string authParms)


        {
            string storedProc;
            OperationType operationType;
            if (entity.BDG_CODE > 0)
            {
                storedProc = "PRC_FINS_BUDGET_TYPES_UPD";
                operationType = OperationType.Update;
            }
            else
            {
                storedProc = "PRC_FINS_BUDGET_TYPES_INS";
                operationType = OperationType.Add;
            }
            var dyParam = GetDynamicParameters(entity, authParms, operationType);
            return await OracleDQ.PostDataAsync(storedProc, authParms, dyParam, commandType: CommandType.StoredProcedure);
        }
        private OracleDynamicParameters GetDynamicParameters(BudgetType entity, string authParms, OperationType operationType)
        {
            var dyParam = new OracleDynamicParameters();
            if (operationType != OperationType.Add)
                dyParam.Add("P_BDG_CODE", OracleDbType.Int32, ParameterDirection.Input, entity.BDG_CODE);

            if (operationType != OperationType.Delete)
            {
                dyParam.Add("P_BDG_NAME_AR", OracleDbType.Varchar2, ParameterDirection.Input, entity.BDG_NAME_AR);
                dyParam.Add("P_BDG_NAME_EN", OracleDbType.Varchar2, ParameterDirection.Input, entity.BDG_NAME_EN);
                dyParam.Add("P_BDG_TXN_CODE", OracleDbType.Varchar2, ParameterDirection.Input, "NO_CODE");
            }
            dyParam.Add("P_USER_ID", OracleDbType.Int32, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserCode);
            dyParam.Add("P_LANG", OracleDbType.Varchar2, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserLanguage);
            dyParam.Add("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
            dyParam.Add("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);

            return dyParam;
        }
    }
}