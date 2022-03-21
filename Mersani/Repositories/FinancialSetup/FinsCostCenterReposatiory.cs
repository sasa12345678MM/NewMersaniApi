using Mersani.Interfaces.FinancialSetup;
using Mersani.models.FinancialSetup;
using Mersani.Oracle;
using Mersani.Utility;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.FinancialSetup
{
    public class FinsCostCenterReposatiory : IFinsCostCeneterRepository
    {
        IConfiguration configuration;
        Database database;
        public FinsCostCenterReposatiory(IConfiguration _configuration, Database _database)
        {
            configuration = _configuration;
            database = _database;
        }

   

        public async Task<DataSet> GetFinCostCenterDetails(FinsCostCeneter entity, string authParms)
        {
            var query = "SELECT xx.* FROM FINS_COST_CENTER xx WHERE xx.COST_CENTER_CODE = :pCOST_CENTER_CODE OR :pCOST_CENTER_CODE = 0";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", entity.COST_CENTER_CODE)
            }, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostFinCostCenter(List<FinsCostCeneter> FinisCostCenter, string authParms)
        {
            foreach (var entity in FinisCostCenter)
            {
                if (entity.COST_CENTER_CODE > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.INS_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_COST_CENTER_XML", FinisCostCenter.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeleteFinCostCenter(FinsCostCeneter entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_COST_CENTER_XML", new List<dynamic>() { entity }, authParms);
        }










    }
}
  