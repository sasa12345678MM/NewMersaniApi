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
    public class CustomerClassRepository : ICustomerClassRepo
    {
        public async Task<DataSet> BulkInsertUpdateCustomerData(List<CustomerClass> entities, string authParms)
        {
            foreach (CustomerClass entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.FCUC_SYS_ID > 0)
                {
                    entity.STATE = (int)OperationType.Update;
                }
                else
                {
                    entity.STATE = (int)OperationType.Add;
                }
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_CUST_CLASS_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> GetCustomerClassDataList(CustomerClass entity, string authParms)
        {
            var query = $"SELECT * FROM FINS_CUST_CLASS WHERE FCUC_SYS_ID = :pFCUC_SYS_ID OR :pFCUC_SYS_ID = 0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pFCUC_SYS_ID", entity.FCUC_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> DeleteCustomerData(CustomerClass entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_CUST_CLASS_XML", new List<dynamic>() { entity }, authParms);
        }
    }
}
