using System;
using System.Data;
using Mersani.Oracle;
using Mersani.models.FinancialSetup;
using System.Collections.Generic;
using Mersani.Interfaces.FinancialSetup;
using Oracle.ManagedDataAccess.Client;

namespace Mersani.Repositories.FinancialSetup
{
    public class FinsAccountLevelRepository: IFinsAccountLevelRepository
    {
        public List<FinsAccountLevel> GetFinAccountLevelDetails(int id, string authParms)
        {
            var query = "SELECT * FROM FINS_ACC_LEVEL  WHERE ACC_LEVEL_CODE = :pACC_LEVEL_CODE OR :pACC_LEVEL_CODE = 0";
            return OracleDQ.GetData<FinsAccountLevel>(query, authParms, new { pACC_LEVEL_CODE = id });
        }

        public bool PostNewFinAccountLevel(FinsAccountLevel fINS_ACC_LEVEL, string authParms)
        {
            var dyParam = GetDynamicParameters(fINS_ACC_LEVEL, authParms, OperationType.Add);
            return OracleDQ.PostData("PRC_FINS_ACC_LEVEL_INS", authParms, dyParam, commandType: CommandType.StoredProcedure);
        }

        public bool UpdateFinAccountLevel(int id, FinsAccountLevel fINS_ACC_LEVEL, string authParms)
        {
            var dyParam = GetDynamicParameters(fINS_ACC_LEVEL, authParms, OperationType.Update);
            return OracleDQ.PostData("PRC_FINS_ACC_LEVEL_UPD", authParms, dyParam, commandType: CommandType.StoredProcedure);
        }

        public bool DeleteFinAccountLevel(int id, string authParms)
        {
            var dyParam = GetDynamicParameters(new FinsAccountLevel() { ACC_LEVEL_CODE = id }, authParms, OperationType.Delete);
            return OracleDQ.PostData("PRC_FINS_ACC_LEVEL_DEL", authParms, dyParam, commandType: CommandType.StoredProcedure);
        }

        public List<FinsAccountLevel> SearchFinsAccountLevel(FinsAcountClass criteria, string authParms)
        {
            throw new NotImplementedException();
        }

        private OracleDynamicParameters GetDynamicParameters(FinsAccountLevel FinsAccountlevel, string authParms, OperationType operationType)
        {
            var dyParam = new OracleDynamicParameters();
           
                dyParam.Add("P_ACC_LEVEL_CODE", OracleDbType.Int32, ParameterDirection.Input, FinsAccountlevel.ACC_LEVEL_CODE);
            if (operationType != OperationType.Delete)
            {
                dyParam.Add("P_ACC_LEVEL_DIGITS", OracleDbType.Int32, ParameterDirection.Input, FinsAccountlevel.ACC_LEVEL_DIGITS);
                dyParam.Add("P_ACC_LEVEL_NAME_AR", OracleDbType.Varchar2, ParameterDirection.Input, FinsAccountlevel.ACC_LEVEL_NAME_AR);
                dyParam.Add("P_ACC_LEVEL_NAME_EN", OracleDbType.Varchar2, ParameterDirection.Input, FinsAccountlevel.ACC_LEVEL_NAME_EN);
            }
            dyParam.Add("P_USER_ID", OracleDbType.Int32, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserCode);
            dyParam.Add("P_LANG", OracleDbType.Varchar2, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserLanguage);
            dyParam.Add("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
            dyParam.Add("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);

            return dyParam;
        }

    }
}
