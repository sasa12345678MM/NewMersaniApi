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
    public class BankSetupRepository : IBankSetupRepo
    {
       

        public async Task<DataSet> GetBankSetup(int id,string authParms)
        {
            var query = $"SELECT * FROM FINS_BANKS  WHERE FB_BANK_CODE = :pCode OR :pCode = 0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pITU_ITEM_SYS_ID", id)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<bool> PostBankSetup(BankSetup entity, string authParms)
        {
            string storedProc;
            OperationType operationType;
            if (entity.FB_BANK_CODE > 0)
            {
                storedProc = "PRC_FINS_BANKS_UPD";
                operationType = OperationType.Update;
            }
            else
            {
                storedProc = "PRC_FINS_BANKS_INS";
                operationType = OperationType.Add;
            }
            var dyParam = GetDynamicParameters(entity, authParms, operationType);
            return await OracleDQ.PostDataAsync(storedProc, authParms, dyParam, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> DeletBankSetup(int id, string Auth)
        {
            var dyParam = GetDynamicParameters(new BankSetup() { FB_BANK_CODE = id }, Auth, OperationType.Delete);
            return await OracleDQ.PostDataAsync("PRC_FINS_BANKS_DEL", Auth, dyParam, commandType: CommandType.StoredProcedure);
        }
    
      
        private OracleDynamicParameters GetDynamicParameters(BankSetup bank, string authParms, OperationType operationType)
        {
            var dyParam = new OracleDynamicParameters();
            if (operationType != OperationType.Add)
            dyParam.Add("P_FB_BANK_CODE", OracleDbType.Int32, ParameterDirection.Input, bank.FB_BANK_CODE);
            if (operationType != OperationType.Delete)
            {
                dyParam.Add("P_FB_BANK_NAME_AR", OracleDbType.Varchar2, ParameterDirection.Input, bank.FB_BANK_NAME_AR);
                dyParam.Add("P_FB_BANK_NAME_EN", OracleDbType.Varchar2, ParameterDirection.Input, bank.FB_BANK_NAME_EN);
                dyParam.Add("P_FB_BANK_IBAN", OracleDbType.Varchar2, ParameterDirection.Input, bank.FB_BANK_IBAN);
                dyParam.Add("P_FB_BANK_TEL", OracleDbType.Varchar2, ParameterDirection.Input, bank.FB_BANK_TEL);
                dyParam.Add("P_FB_BANK_FAX", OracleDbType.Varchar2, ParameterDirection.Input, bank.FB_BANK_FAX);
                dyParam.Add("P_FB_CNTC_PERSION", OracleDbType.Varchar2, ParameterDirection.Input, bank.FB_CNTC_PERSION);
                dyParam.Add("P_FB_CNTC_INFO", OracleDbType.Varchar2, ParameterDirection.Input, bank.FB_CNTC_INFO);
            }
            dyParam.Add("P_USER_ID", OracleDbType.Int32, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserCode);
            dyParam.Add("P_LANG", OracleDbType.Varchar2, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserLanguage);
            dyParam.Add("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
            dyParam.Add("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);

            return dyParam;
        }

    }
}


