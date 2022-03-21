using System.Data;
using Mersani.Oracle;
using Mersani.models.FinancialSetup;
using System.Collections.Generic;
using Mersani.Interfaces.FinancialSetup;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;

namespace Mersani.Repositories.FinancialSetup
{
    public class FinsAccountRepository : IFinisAccountRepository
    {
        public async Task<DataSet> PostFinsAccount(FinsAccount entity, string authParms)
        {
            if (entity.ACC_CODE > 0)
            {
                entity.STATE = (int)OperationType.Update;
            }
            else
            {
                entity.STATE = (int)OperationType.Add;
            }
            entity.INS_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_ACCOUNT_XML", new List<dynamic>() { entity }, authParms);
        }
        public async Task<DataSet> DeletFinsAccount(FinsAccount entity, string authParms)
        {
            entity.INS_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_ACCOUNT_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetFinsAccount(FinsAccount entity, string authParms)
        {
            var query = "SELECT * FROM FINS_ACCOUNT WHERE FINS_ACCOUNT.ACC_LEVEL_CODE = 1 ORDER BY ACC_NO ASC";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetFinsAccountChildern(FinsAccount entity, string authParms)
        {
            var query = "SELECT * FROM FINS_ACCOUNT WHERE PARENT_ACC_NO = :pPARENT_ACC_NO ORDER BY ACC_NO ASC";
            var parms = new List<OracleParameter>() { new OracleParameter("pPARENT_ACC_NO", entity.ACC_NO) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetAccountNoTwoByParent(FinsAccount entity, string authParms)
        {
            var query = $"SELECT new_acc_no2(:pPARENT_ACC_CODE, :pLevel, :pVCode) AS ACC_NO FROM DUAL";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pPARENT_ACC_CODE", entity.PARENT_ACC_CODE),
                new OracleParameter("pLevel", entity.ACC_LEVEL_CODE),
                new OracleParameter("pVCode", entity.ACC_V_CODE)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetAccountNoThreeByParent(FinsAccount entity, string authParms)
        {
            var query = $"SELECT new_acc_no3(:pPARENT_ACC_CODE, :pVCode) AS ACC_NO FROM DUAL";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pPARENT_ACC_CODE", entity.PARENT_ACC_CODE),
                new OracleParameter("pVCode", entity.ACC_V_CODE)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
    }

}