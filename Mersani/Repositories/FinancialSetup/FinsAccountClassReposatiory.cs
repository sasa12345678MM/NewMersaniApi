using Mersani.Interfaces.FinancialSetup;
using Mersani.models.FinancialSetup;
using Mersani.Oracle;
using Mersani.Utility;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Mersani.Repositories.FinancialSetup
{
    public class FinsAccountClassReposatiory : IFinsAccountClassRepository
    {
        IConfiguration configuration;
        Database database;
        public FinsAccountClassReposatiory(IConfiguration _configuration, Database _database)
        {
            configuration = _configuration;
            database = _database;
        }
        bool IFinsAccountClassRepository.DeleteFINS_ACC_CLASS(int id, string authParms)
        {
            var dyParam = GetDynamicParameters(new FinsAcountClass() { ACC_CLASS_CODE = id }, authParms, OperationType.Delete);
            return OracleDQ.PostData("PRC_FINS_ACC_CLASS_DEL", authParms, dyParam, commandType: CommandType.StoredProcedure);
        }

        List<FinsAcountClass> IFinsAccountClassRepository.GetFINS_ACC_CLASSDetails(int id, string authParms)
        {
            var query = "SELECT * FROM FINS_ACC_CLASS  WHERE ACC_CLASS_CODE = :pACC_CLASS_CODE OR :pACC_CLASS_CODE = 0";
            return OracleDQ.GetData<FinsAcountClass>(query, authParms, new { pACC_CLASS_CODE = id });
        }
         
        bool IFinsAccountClassRepository.PostNewFINS_ACC_CLASS(FinsAcountClass finsAccountClass, string authParms)
        {
            var dyParam = GetDynamicParameters(finsAccountClass, authParms, OperationType.Add);
            return OracleDQ.PostData("PRC_FINS_ACC_CLASS_INS", authParms, dyParam, commandType: CommandType.StoredProcedure);
        }

        List<FinsAcountClass> IFinsAccountClassRepository.SearchFinsAccountClass(FinsAcountClass criteria, string authParms)
        {
            throw new NotImplementedException();
        }

        bool IFinsAccountClassRepository.UpdateFINS_ACC_CLASS(int id, FinsAcountClass finsAccountClass, string authParms)
        {
            var dyParam = GetDynamicParameters(finsAccountClass, authParms, OperationType.Update);
            return OracleDQ.PostData("PRC_FINS_ACC_CLASS_UPD", authParms, dyParam, commandType: CommandType.StoredProcedure);
        }
    
        private OracleDynamicParameters GetDynamicParameters(FinsAcountClass FinisAccountClass, string authParms, OperationType operationType)
        {
            var dyParam = new OracleDynamicParameters();
           
                dyParam.Add("P_ACC_CLASS_CODE", OracleDbType.Int32, ParameterDirection.Input, FinisAccountClass.ACC_CLASS_CODE);
            
            if (operationType != OperationType.Delete)
            {

                dyParam.Add("P_ACC_CLASS_NAME_AR", OracleDbType.Varchar2, ParameterDirection.Input, FinisAccountClass.ACC_CLASS_NAME_AR);
                dyParam.Add("P_ACC_CLASS_NAME_EN", OracleDbType.Varchar2, ParameterDirection.Input, FinisAccountClass.ACC_CLASS_NAME_EN);
            }
            dyParam.Add("P_USER_ID", OracleDbType.Int32, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserCode);
            dyParam.Add("P_LANG", OracleDbType.Varchar2, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserLanguage);
            dyParam.Add("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
            dyParam.Add("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);
            return dyParam;
        }
    }
}
